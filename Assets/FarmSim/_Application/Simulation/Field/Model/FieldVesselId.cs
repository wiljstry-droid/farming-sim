using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure identity value for a FieldVessel.
    /// Data-only: no simulation behavior.
    /// </summary>
    [Serializable]
    public readonly struct FieldVesselId : IEquatable<FieldVesselId>
    {
        public readonly string value;

        public FieldVesselId(string value)
        {
            this.value = value ?? string.Empty;
        }

        public static FieldVesselId New()
        {
            return new FieldVesselId(Guid.NewGuid().ToString("N"));
        }

        public bool Equals(FieldVesselId other)
        {
            return string.Equals(value, other.value, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            return obj is FieldVesselId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return value != null ? StringComparer.Ordinal.GetHashCode(value) : 0;
        }

        public override string ToString()
        {
            return value ?? string.Empty;
        }

        public static bool operator ==(FieldVesselId a, FieldVesselId b) => a.Equals(b);
        public static bool operator !=(FieldVesselId a, FieldVesselId b) => !a.Equals(b);
    }
}
