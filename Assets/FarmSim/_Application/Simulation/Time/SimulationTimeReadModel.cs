using System;

namespace FarmSim.Application.Simulation.Time
{
    public sealed class SimulationTimeReadModel
    {
        public DateTime UtcNow { get; }
        public SimulationSpeedMode SpeedMode { get; }

        public SimulationTimeReadModel(DateTime utcNow, SimulationSpeedMode speedMode)
        {
            UtcNow = DateTime.SpecifyKind(utcNow, DateTimeKind.Utc);
            SpeedMode = speedMode;
        }
    }
}
