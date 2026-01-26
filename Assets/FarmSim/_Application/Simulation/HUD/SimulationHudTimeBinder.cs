using UnityEngine;
using FarmSim.Application.Simulation.Time;

namespace FarmSim.Application.Simulation.HUD
{
    public sealed class SimulationHudTimeBinder : MonoBehaviour
    {
        [SerializeField] private SimulationHudView view;
        [SerializeField] private SimulationTimeAuthorityReadModelPublisher timePublisher;

        private void Update()
        {
            if (view == null || timePublisher == null)
                return;

            var rm = timePublisher.Current;
            if (rm == null)
                return;

            view.SetSimTime($"SIM TIME: {rm.UtcNow:O} ({rm.SpeedMode})");
        }
    }
}
