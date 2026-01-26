using System;
using FarmSim.Application.Simulation.Step.Model;

namespace FarmSim.Application.Simulation.Step
{
    /// <summary>
    /// Minimal Tier-1 soil moisture "bucket" core.
    /// Deterministic, engine-agnostic. No Unity types.
    /// </summary>
    public sealed class SoilMoistureBucketCore
    {
        private readonly SoilMoistureBucketState state = new SoilMoistureBucketState();

        // Report once per simulated second (same pattern as FirstSimulationStepCore).
        private double nextReportAtSeconds = 1d;

        public SoilMoistureBucketState State => state;

        /// <summary>
        /// Advances authoritative bucket state.
        /// Returns true when a report point is crossed (once per simulated second).
        /// </summary>
        public bool Step(double deltaSeconds)
        {
            if (deltaSeconds <= 0d)
                return false;

            state.Advance(deltaSeconds);

            // Deterministic forcing:
            // - "Rain pulse" every 12 simulated seconds
            // - Constant ET every second
            // This is intentionally simple but real: moisture responds to inputs.
            var t = state.TotalSimSeconds;

            var rainPulse = IsRainPulse(t) ? 0.10d : 0.00d; // +0.10 on pulse
            var evap = 0.02d * deltaSeconds;               // -0.02 per simulated second

            var moisture = state.Moisture01;
            moisture += rainPulse;
            moisture -= evap;

            state.SetMoisture01(moisture);

            if (state.TotalSimSeconds >= nextReportAtSeconds)
            {
                nextReportAtSeconds = Math.Floor(state.TotalSimSeconds) + 1d;
                return true;
            }

            return false;
        }

        private static bool IsRainPulse(double simSeconds)
        {
            // Pulse at t = 2, 14, 26, ... (i.e., once per 12 seconds, offset for visibility)
            var k = (int)Math.Floor(simSeconds);
            return (k % 12) == 2;
        }
    }
}
