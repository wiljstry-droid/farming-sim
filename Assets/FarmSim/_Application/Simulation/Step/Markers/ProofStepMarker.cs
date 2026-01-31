using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Markers
{
    /// <summary>
    /// Pure marker component. No behavior.
    /// Proof steps must be removable.
    /// </summary>
    public sealed class ProofStepMarker : MonoBehaviour, ISimulationProofStep
    {
        [SerializeField] private MonoBehaviour stepBehaviour; // must implement ISimulationStep

        public string StepId
        {
            get
            {
                if (stepBehaviour is ISimulationStep s) return s.StepId;
                return "marker.proof.invalid";
            }
        }

        public void Execute(ISimulationStepContext context)
        {
            // Delegation only.
            if (stepBehaviour is ISimulationStep s)
                s.Execute(context);
        }
    }
}
