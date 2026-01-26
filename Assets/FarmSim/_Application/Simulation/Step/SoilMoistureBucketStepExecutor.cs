using System.Globalization;
using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;
using FarmSim.Application.Simulation.Step.Model;

namespace FarmSim.Application.Simulation.Step
{
    public sealed class SoilMoistureBucketStepExecutor
        : MonoBehaviour, ISimulationStep, ISimulationTickExecutor
    {
        private SoilMoistureBucketCore core;
        private bool loggedFirst;

        public string StepId => "ZZ.SoilMoistureBucket";

        // 🔒 READ-ONLY SURFACE FOR HUD (strongly typed)
        public SoilMoistureBucketState CurrentState => core?.State;

        private void Awake()
        {
            core = new SoilMoistureBucketCore();
        }

        public void Execute(ISimulationStepContext context)
        {
            ExecuteCore((float)context.DeltaSeconds);
        }

        public void Execute(in SimulationTickSnapshot snapshot)
        {
            ExecuteCore(snapshot.deltaSeconds);
        }

        private void ExecuteCore(float deltaSeconds)
        {
            if (deltaSeconds <= 0f)
                return;

            if (!loggedFirst)
            {
                loggedFirst = true;
                Debug.Log("[SoilBucket] First soil moisture bucket tick executed.");
            }

            var report = core.Step(deltaSeconds);

            if (report)
            {
                var s = core.State;

                Debug.Log(string.Format(
                    CultureInfo.InvariantCulture,
                    "[SoilBucket] ticks={0} simSeconds={1:0.000} moisture={2:0.000} stressed={3} aboveFC={4}",
                    s.TotalTicks,
                    s.TotalSimSeconds,
                    s.Moisture01,
                    s.IsWaterStressed,
                    s.IsAboveFieldCapacity
                ));
            }
        }
    }
}
