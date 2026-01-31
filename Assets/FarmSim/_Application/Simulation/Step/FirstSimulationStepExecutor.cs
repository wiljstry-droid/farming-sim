using System.Globalization;
using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Step
{
    public sealed class FirstSimulationStepExecutor : MonoBehaviour, ISimulationStep, ISimulationTickExecutor
    {
        private FirstSimulationStepCore core;
        private bool loggedFirst;

        public string StepId => "00.FirstSimulationStep";

        private void Awake()
        {
            core = new FirstSimulationStepCore();
        }

        // Step pipeline entry point
        public void Execute(ISimulationStepContext context)
        {
            ExecuteCore((float)context.DeltaSeconds);
        }

        // Tick execution entry point (authoritative)
        public void Execute(in SimulationTickSnapshot snapshot)
        {
            ExecuteCore(snapshot.deltaSeconds);
        }

        private void ExecuteCore(float deltaSeconds)
        {
            if (deltaSeconds <= 0f) return;

            if (!loggedFirst)
            {
                loggedFirst = true;
                Debug.Log("[FirstStep] First Tier-1 simulation step executed.");
            }

            var report = core.Step(deltaSeconds);
            if (report)
            {
                var s = core.State;
                Debug.Log(string.Format(
                    CultureInfo.InvariantCulture,
                    "[FirstStep] ticks={0} simSeconds={1:0.000}",
                    s.TotalTicks,
                    s.TotalSimSeconds
                ));
            }
        }
    }
}
