using System;

namespace FarmSim.Application.Simulation.Time
{
    public sealed class SimulationClock
    {
        public DateTime UtcNow { get; private set; }

        public SimulationClock(DateTime utcStart)
        {
            UtcNow = DateTime.SpecifyKind(utcStart, DateTimeKind.Utc);
        }

        public void Advance(TimeSpan delta)
        {
            if (delta <= TimeSpan.Zero)
                return;

            UtcNow = UtcNow.Add(delta);
        }

        public void SetUtc(DateTime utc)
        {
            UtcNow = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
        }
    }
}
