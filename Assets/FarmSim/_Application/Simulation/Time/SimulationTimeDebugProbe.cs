using UnityEngine;

namespace FarmSim.Application.Simulation.Time
{
    public sealed class SimulationTimeDebugProbe : MonoBehaviour
    {
        [SerializeField]
        private SimulationTimeAuthorityReadModelPublisher publisher;

        private float nextLogAt;

        private void Awake()
        {
            if (publisher == null)
                publisher = GetComponent<SimulationTimeAuthorityReadModelPublisher>();

            Debug.Log(publisher == null
                ? "[SimTime] DebugProbe Awake: publisher NOT found on this GameObject."
                : "[SimTime] DebugProbe Awake: publisher bound.");
        }

        private void Update()
        {
            if (publisher == null)
                return;

            if (publisher.Current == null)
                return;

            if (UnityEngine.Time.unscaledTime < nextLogAt)
                return;

            nextLogAt = UnityEngine.Time.unscaledTime + 1f;

            var rm = publisher.Current;
            Debug.Log($"[SimTime] UtcNow={rm.UtcNow:O} Speed={rm.SpeedMode}");
        }
    }
}
