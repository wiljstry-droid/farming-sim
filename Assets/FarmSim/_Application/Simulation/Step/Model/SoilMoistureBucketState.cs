namespace FarmSim.Application.Simulation.Step.Model
{
    /// <summary>
    /// Minimal Tier-1 authoritative state for a single "test" soil bucket.
    /// Engine-agnostic. No Unity types.
    /// </summary>
    public sealed class SoilMoistureBucketState
    {
        public ulong TotalTicks { get; private set; }
        public double TotalSimSeconds { get; private set; }

        /// <summary>
        /// Normalized soil moisture in [0..1].
        /// </summary>
        public double Moisture01 { get; private set; } = 0.55d;

        /// <summary>
        /// Thresholds expressed in the same 0..1 normalized space.
        /// (These are placeholders for now; values chosen for clear behavior in logs.)
        /// </summary>
        public double WiltingPoint01 { get; } = 0.20d;
        public double FieldCapacity01 { get; } = 0.60d;

        public bool IsWaterStressed => Moisture01 <= WiltingPoint01;
        public bool IsAboveFieldCapacity => Moisture01 >= FieldCapacity01;

        public void Advance(double deltaSeconds)
        {
            if (deltaSeconds <= 0d)
                return;

            TotalTicks++;
            TotalSimSeconds += deltaSeconds;
        }

        public void SetMoisture01(double value01)
        {
            if (value01 < 0d) value01 = 0d;
            if (value01 > 1d) value01 = 1d;
            Moisture01 = value01;
        }
    }
}
