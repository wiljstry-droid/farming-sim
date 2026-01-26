using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Model;

namespace FarmSim.Application.Simulation.Step.Diagnostics
{
    /// <summary>
    /// Deterministic proof step for Phase 24:
    /// - Implements a gate that always denies.
    /// - Provides stable reason code + optional detail.
    /// - Ensures denied steps produce Denied outcomes in the tick snapshot.
    ///
    /// Foundation-only diagnostics step. Safe to delete later.
    /// </summary>
    public sealed class DebugGateDeniedStep : MonoBehaviour, ISimulationStep, ISimulationStepGate
    {
        [SerializeField] private string stepId = "Debug.GateDenied.Phase24";

        public string StepId => stepId;

        public bool IsAllowed(out string reasonCode, out string reasonDetail)
        {
            reasonCode = SimulationStepGateReasonCodes.NotReady;
            reasonDetail = "DebugGateDeniedStep (Phase24 proof)";
            return false;
        }

        // Must never run when gate denies.
        public void Execute(ISimulationStepContext context)
        {
            Debug.LogError("[DebugGateDeniedStep] Execute ran but should have been denied.");
        }
    }
}
