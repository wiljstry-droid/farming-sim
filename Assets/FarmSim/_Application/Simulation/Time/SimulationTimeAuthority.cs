using System;
using UnityEngine;
using FarmSim.Application.Simulation.Time.Contracts;

namespace FarmSim.Application.Simulation.Time
{
    public sealed class SimulationTimeAuthority : MonoBehaviour, ISimulationTimeDeltaSource
    {
        public static SimulationTimeAuthority Instance { get; private set; }

        [SerializeField]
        private SimulationSpeedMode speedMode = SimulationSpeedMode.Normal;

        private SimulationClock clock;

        private float _deltaTimeSeconds;

        public DateTime UtcNow => clock.UtcNow;
        public SimulationSpeedMode SpeedMode => speedMode;

        // ISimulationTimeDeltaSource
        public float DeltaTimeSeconds => _deltaTimeSeconds;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            clock = new SimulationClock(DateTime.UtcNow);
            _deltaTimeSeconds = 0f;
        }

        private void Update()
        {
            var unscaled = UnityEngine.Time.unscaledDeltaTime;

            if (speedMode == SimulationSpeedMode.Paused || unscaled <= 0f)
            {
                _deltaTimeSeconds = 0f;
                return;
            }

            float multiplier = speedMode switch
            {
                SimulationSpeedMode.Normal => 1f,
                SimulationSpeedMode.Fast => 5f,
                SimulationSpeedMode.Ultra => 30f,
                _ => 1f
            };

            _deltaTimeSeconds = unscaled * multiplier;

            if (_deltaTimeSeconds > 0f)
                clock.Advance(TimeSpan.FromSeconds(_deltaTimeSeconds));
        }
    }
}
