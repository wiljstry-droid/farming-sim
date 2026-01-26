using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure construction helper for FieldVesselData.
    /// Data-only: no simulation behavior, no Unity dependencies.
    /// </summary>
    public static class FieldVesselFactory
    {
        public static FieldVesselData CreateNew(string label, long createdUtcSeconds)
        {
            return new FieldVesselData(
                FieldVesselId.New(),
                label ?? string.Empty,
                FieldVesselLifecycleState.Active,
                createdUtcSeconds);
        }

        public static FieldVesselData CreateNewNow(string label)
        {
            long utcSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return CreateNew(label, utcSeconds);
        }
    }
}
