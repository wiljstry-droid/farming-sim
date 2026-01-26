using UnityEngine;

namespace FarmSim.Application.Simulation.Time
{
    public sealed class SimulationTimeAuthorityReadModelPublisher : MonoBehaviour
    {
        [SerializeField]
        private SimulationTimeAuthority source;

        public SimulationTimeReadModel Current { get; private set; }

        private void Awake()
        {
            if (source == null)
                source = GetComponent<SimulationTimeAuthority>();

            Debug.Log(source == null
                ? "[SimTime] ReadModelPublisher Awake: source NOT found on same GameObject."
                : "[SimTime] ReadModelPublisher Awake: source bound (same GO).");
        }

        private void Update()
        {
            if (source == null)
                return;

            Current = new SimulationTimeReadModel(source.UtcNow, source.SpeedMode);
        }
    }
}
