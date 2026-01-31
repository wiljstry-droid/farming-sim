using System;
using System.Collections.Generic;
using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Model;
using FarmSim.Application.Simulation.Tick.Model;
using FarmSim.Application.Simulation.Tick.Model.Governance;
using FarmSim.Application.Simulation.Time;
using FarmSim.Application.Simulation.Time.Contracts;
using FarmSim.Application.Simulation.Tick.Contracts;

namespace FarmSim.Application.Simulation.Tick
{
    public sealed class SimulationTickAuthority : MonoBehaviour
    {
        public static SimulationTickAuthority Instance { get; private set; }

        private ISimulationTimeDeltaSource timeDeltaSource;
        private readonly SimulationTickGate gate = new SimulationTickGate();

        private bool loggedCoupling;

        private bool loggedGateBlocked;
        private bool loggedNoDeltaSource;
        private bool loggedZeroDelta;

        [Header("Outcome Governance (Phase 27)")]
        [SerializeField] private bool strictOutcomeGovernanceLogs = false;

        private long tickIndex;

        private ISimulationStep[] orderedStepsCache;
        private ISimulationStep[] discoveredStepsCache;

        private ISimulationTickExecutor boundExecutor;

        public SimulationTickExecutionSnapshot LastExecutionSnapshot { get; private set; }

        public void BindExecutor(ISimulationTickExecutor executor)
        {
            boundExecutor = executor;
        }

        public void BindTimeDeltaSource(ISimulationTimeDeltaSource source)
        {
            timeDeltaSource = source;
            loggedNoDeltaSource = false;
            loggedZeroDelta = false;
        }

        public void SetPaused(bool paused)
        {
            gate.SetPaused(paused);
            loggedGateBlocked = false;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            if (timeDeltaSource == null)
            {
                timeDeltaSource = SimulationTimeAuthority.Instance;
            }

            if (!loggedCoupling)
            {
                loggedCoupling = true;

                if (timeDeltaSource == null)
                {
                    Debug.LogWarning("[Tick] Coupling: SimulationTimeAuthority.Instance not available (timeDeltaSource is null).");
                }
                else
                {
                    Debug.Log("[Tick] Coupling: timeDeltaSource bound.");
                }
            }
        }

        private void Update()
        {
            if (discoveredStepsCache == null)
            {
                discoveredStepsCache = GetComponentsInChildren<ISimulationStep>(true);
                RebuildExecutedStepsCache();
            }

            if (!gate.CanTick())
            {
                if (!loggedGateBlocked)
                {
                    loggedGateBlocked = true;
                    Debug.Log("[Tick] Blocked by SimulationTickGate (paused or gated).");
                }
                return;
            }

            if (timeDeltaSource == null)
            {
                if (!loggedNoDeltaSource)
                {
                    loggedNoDeltaSource = true;
                    Debug.LogError("[Tick] No ISimulationTimeDeltaSource bound. Tick cannot run.");
                }
                return;
            }

            float deltaSeconds = (float)timeDeltaSource.DeltaTimeSeconds;

            if (deltaSeconds <= 0f)
            {
                if (!loggedZeroDelta)
                {
                    loggedZeroDelta = true;
                    Debug.Log("[Tick] DeltaTimeSeconds <= 0. Tick will not advance.");
                }
                return;
            }

            loggedGateBlocked = false;
            loggedNoDeltaSource = false;
            loggedZeroDelta = false;

            tickIndex++;

            if (boundExecutor != null)
            {
                var snapshot = new SimulationTickSnapshot(deltaSeconds);
                boundExecutor.Execute(in snapshot);
            }

            var context = new SimulationStepContext(tickIndex, deltaSeconds);

            var records = new List<SimulationStepExecutionRecord>();

            if (orderedStepsCache == null)
            {
                RebuildExecutedStepsCache();
            }

            for (int i = 0; i < orderedStepsCache.Length; i++)
            {
                var step = orderedStepsCache[i];
                if (step == null)
                    continue;

                if (step is ISimulationStepGate gateStep)
                {
                    string reasonCode;
                    string reasonDetail;

                    bool allowed = false;

                    try
                    {
                        allowed = gateStep.IsAllowed(out reasonCode, out reasonDetail);
                    }
                    catch (Exception ex)
                    {
                        allowed = false;
                        reasonCode = SimulationStepGateReasonCodes.InvalidState;
                        reasonDetail = ex.GetType().Name;
                    }

                    if (!allowed)
                    {
                        if (string.IsNullOrWhiteSpace(reasonCode))
                            reasonCode = SimulationStepGateReasonCodes.Unspecified;

                        var deniedOutcome = SimulationStepOutcome.Denied(reasonCode, reasonDetail);

                        var deniedGovViolation = SimulationStepOutcomeGovernance.Validate(deniedOutcome);
                        if (deniedGovViolation.IsViolation)
                        {
                            var msg = "[OutcomeGov] step=" + step.StepId +
                                      " outcome=" + deniedOutcome +
                                      " violation=" + deniedGovViolation.Kind +
                                      " detail=" + deniedGovViolation.Detail;
                            if (strictOutcomeGovernanceLogs) Debug.LogError(msg); else Debug.LogWarning(msg);
                        }

                        records.Add(new SimulationStepExecutionRecord(step, deniedOutcome));
                        continue;
                    }
                }

                if (step is ISimulationStepWithOutcome outcomeStep)
                {
                    SimulationStepOutcome outcome;

                    try
                    {
                        outcome = outcomeStep.ExecuteWithOutcome(context);
                    }
                    catch (Exception ex)
                    {
                        outcome = SimulationStepOutcome.Failed(ex);
                    }

                    if (outcome == null)
                        outcome = SimulationStepOutcome.Failed(new InvalidOperationException("Step returned null outcome."));

                    var govViolation = SimulationStepOutcomeGovernance.Validate(outcome);
                    if (govViolation.IsViolation)
                    {
                        var msg = "[OutcomeGov] step=" + step.StepId +
                                  " outcome=" + outcome +
                                  " violation=" + govViolation.Kind +
                                  " detail=" + govViolation.Detail;
                        if (strictOutcomeGovernanceLogs) Debug.LogError(msg); else Debug.LogWarning(msg);
                    }

                    records.Add(new SimulationStepExecutionRecord(step, outcome));

                    if (outcome.Kind == SimulationStepOutcomeKind.Failed)
                        break;

                    continue;
                }

                try
                {
                    step.Execute(context);
                    var legacyOutcome = SimulationStepOutcome.Success();

                    var legacyGovViolation = SimulationStepOutcomeGovernance.Validate(legacyOutcome);
                    if (legacyGovViolation.IsViolation)
                    {
                        var msg = "[OutcomeGov] step=" + step.StepId +
                                  " outcome=" + legacyOutcome +
                                  " violation=" + legacyGovViolation.Kind +
                                  " detail=" + legacyGovViolation.Detail;
                        if (strictOutcomeGovernanceLogs) Debug.LogError(msg); else Debug.LogWarning(msg);
                    }

                    records.Add(new SimulationStepExecutionRecord(step, legacyOutcome));
                }
                catch (Exception ex)
                {
                    var legacyFailOutcome = SimulationStepOutcome.Failed(ex);

                    var legacyFailGovViolation = SimulationStepOutcomeGovernance.Validate(legacyFailOutcome);
                    if (legacyFailGovViolation.IsViolation)
                    {
                        var msg = "[OutcomeGov] step=" + step.StepId +
                                  " outcome=" + legacyFailOutcome +
                                  " violation=" + legacyFailGovViolation.Kind +
                                  " detail=" + legacyFailGovViolation.Detail;
                        if (strictOutcomeGovernanceLogs) Debug.LogError(msg); else Debug.LogWarning(msg);
                    }

                    records.Add(new SimulationStepExecutionRecord(step, legacyFailOutcome));
                    break;
                }
            }

            LastExecutionSnapshot = new SimulationTickExecutionSnapshot(tickIndex, deltaSeconds, records);
        }

        private void RebuildExecutedStepsCache()
        {
            if (discoveredStepsCache == null)
            {
                orderedStepsCache = Array.Empty<ISimulationStep>();
                return;
            }

            // If any authoritative steps exist, execute ONLY those.
            // Proof steps are always excluded.
            var authoritative = new List<ISimulationStep>();
            bool foundAnyAuthoritative = false;

            for (int i = 0; i < discoveredStepsCache.Length; i++)
            {
                var s = discoveredStepsCache[i];
                if (s == null)
                    continue;

                if (s is ISimulationProofStep)
                    continue;

                if (s is ISimulationAuthoritativeStep)
                {
                    foundAnyAuthoritative = true;
                    authoritative.Add(s);
                }
            }

            if (foundAnyAuthoritative)
            {
                orderedStepsCache = authoritative.ToArray();
                return;
            }

            // Legacy fallback until tagging/wiring is applied.
            orderedStepsCache = discoveredStepsCache;
        }
    }
}
