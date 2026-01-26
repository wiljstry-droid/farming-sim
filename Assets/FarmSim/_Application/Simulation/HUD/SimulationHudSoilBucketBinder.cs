using System.Globalization;
using UnityEngine;
using FarmSim.Application.Simulation.Step;

namespace FarmSim.Application.Simulation.HUD
{
    public sealed class SimulationHudSoilBucketBinder : MonoBehaviour
    {
        [SerializeField] private SimulationHudView view;
        [SerializeField] private SoilMoistureBucketStepExecutor soilBucketExecutor;

        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private void Update()
        {
            if (view == null || soilBucketExecutor == null)
                return;

            var s = soilBucketExecutor.CurrentState;
            if (s == null)
                return;

            view.SetSoilBucket(string.Format(
                Inv,
                "SOIL BUCKET: moisture={0:0.000} stressed={1} aboveFC={2}",
                s.Moisture01,
                s.IsWaterStressed,
                s.IsAboveFieldCapacity
            ));
        }
    }
}
