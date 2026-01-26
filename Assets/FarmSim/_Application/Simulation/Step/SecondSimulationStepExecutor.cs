using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Diagnostics;
using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Step
{
    /// <summary>
    /// Second Tier-1 demo simulation step.
    /// </summary>
    public sealed class SecondSimulationStepExecutor :
        MonoBehaviour,
        ISimulationStep,
        ISimulationTickExecutor
    {
        private bool loggedFirst;

        public string StepId => "10.SecondSimulationStep";

        public void Execute(ISimulationStepContext context)
        {
            // No-op for now (step pipeline visibility only)
        }

        public void Execute(in SimulationTickSnapshot snapshot)
        {
            if (!loggedFirst)
            {
                loggedFirst = true;
                Debug.Log("[SecondStep] Second Tier-1 simulation step executed.");
            }
        }
    }
}
