using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Markers
{
    /// <summary>
    /// Pure marker component. No behavior.
    /// Used to identify authoritative tick steps in a sterility-safe way.
    /// </summary>
    public sealed class AuthoritativeStepMarker : MonoBehaviour, ISimulationAuthoritativeStep
    {
        [SerializeField] private MonoBehaviour stepBehaviour; // must implement ISimulationStep

        public string StepId
        {
            get
            {
                if (stepBehaviour is ISimulationStep s) return s.StepId;
                return "marker.authoritative.invalid";
            }
        }

        public void Execute(ISimulationStepContext context)
        {
            // Delegation only; authoritative behavior stays in the real step.
            if (stepBehaviour is ISimulationStep s)
                s.Execute(context);
        }
    }
}
