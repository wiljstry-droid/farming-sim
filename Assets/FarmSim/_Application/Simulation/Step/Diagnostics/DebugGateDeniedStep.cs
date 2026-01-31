#nullable enable

using UnityEngine;

using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Diagnostics
{
    /// <summary>
    /// Phase 24 proof step: this step is expected to be denied by the tick gate.
    /// If Execute ever runs, gating failed (purely diagnostic).
    /// </summary>
    public sealed class DebugGateDeniedStep : MonoBehaviour, ISimulationStep
    {
        // Must match the log you already see:
        // [OutcomeGov] step=Debug.GateDenied.Phase24 ...
        public string StepId => "Debug.GateDenied.Phase24";

        // IMPORTANT: Policy-valid token: no spaces, no parentheses.
        // Use this wherever the denial ReasonDetail is set.
        public const string ReasonDetailToken = "DebugGateDeniedStep.Phase24_proof";

        public void Execute(ISimulationStepContext context)
        {
            // If you see this, the gate did NOT deny as expected.
            Debug.LogError("[DebugGateDeniedStep] Execute ran but should have been denied by the gate.");
        }
    }
}
