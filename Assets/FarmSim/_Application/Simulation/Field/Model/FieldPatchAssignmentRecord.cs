using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data identifier for a persistent 1m x 1m FieldPatch in patch-space coordinates.
    /// (No behavior beyond value semantics.)
    /// </summary>
    [Serializable]
    public readonly struct FieldPatchCoord : IEquatable<FieldPatchCoord>
    {
        public readonly int x;
        public readonly int z;

        public FieldPatchCoord(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public bool Equals(FieldPatchCoord other) => x == other.x && z == other.z;
        public override bool Equals(object obj) => obj is FieldPatchCoord other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ z;
            }
        }

        public static bool operator ==(FieldPatchCoord a, FieldPatchCoord b) => a.Equals(b);
        public static bool operator !=(FieldPatchCoord a, FieldPatchCoord b) => !a.Equals(b);

        public override string ToString() => $"Patch({x},{z})";
    }

    /// <summary>
    /// Pure data record describing a historical assignment of a single FieldPatch to a FieldVessel.
    /// History is preserved by ending assignments (endedUtcSeconds) rather than deleting.
    /// </summary>
    [Serializable]
    public readonly struct FieldPatchAssignmentRecord : IEquatable<FieldPatchAssignmentRecord>
    {
        public readonly FieldVesselId vesselId;
        public readonly FieldPatchCoord patch;

        /// <summary>
        /// Assignment start timestamp expressed as UTC seconds (data-only; source system defined later).
        /// </summary>
        public readonly long createdUtcSeconds;

        /// <summary>
        /// Assignment end timestamp expressed as UTC seconds; null means currently active.
        /// </summary>
        public readonly long? endedUtcSeconds;

        public FieldPatchAssignmentRecord(
            FieldVesselId vesselId,
            FieldPatchCoord patch,
            long createdUtcSeconds,
            long? endedUtcSeconds)
        {
            this.vesselId = vesselId;
            this.patch = patch;
            this.createdUtcSeconds = createdUtcSeconds;
            this.endedUtcSeconds = endedUtcSeconds;
        }

        public bool IsActive => !endedUtcSeconds.HasValue;

        public bool Equals(FieldPatchAssignmentRecord other)
        {
            return vesselId.Equals(other.vesselId)
                && patch.Equals(other.patch)
                && createdUtcSeconds == other.createdUtcSeconds
                && endedUtcSeconds == other.endedUtcSeconds;
        }

        public override bool Equals(object obj) => obj is FieldPatchAssignmentRecord other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = vesselId.GetHashCode();
                hash = (hash * 397) ^ patch.GetHashCode();
                hash = (hash * 397) ^ createdUtcSeconds.GetHashCode();
                hash = (hash * 397) ^ (endedUtcSeconds.HasValue ? endedUtcSeconds.Value.GetHashCode() : 0);
                return hash;
            }
        }

        public static bool operator ==(FieldPatchAssignmentRecord a, FieldPatchAssignmentRecord b) => a.Equals(b);
        public static bool operator !=(FieldPatchAssignmentRecord a, FieldPatchAssignmentRecord b) => !a.Equals(b);
    }
}
