using System;
using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data snapshot describing assignment state at a specific instant.
    /// This is a read-only view, not an authority, and carries no behavior.
    /// </summary>
    [Serializable]
    public sealed class FieldPatchAssignmentSnapshot
    {
        /// <summary>
        /// Timestamp (UTC seconds) this snapshot represents.
        /// </summary>
        public readonly long snapshotUtcSeconds;

        /// <summary>
        /// Active patch-to-vessel assignments at snapshot time.
        /// Key: FieldPatchCoord, Value: FieldVesselId
        /// </summary>
        public readonly Dictionary<FieldPatchCoord, FieldVesselId> activePatchOwners;

        public FieldPatchAssignmentSnapshot(
            long snapshotUtcSeconds,
            Dictionary<FieldPatchCoord, FieldVesselId> activePatchOwners)
        {
            this.snapshotUtcSeconds = snapshotUtcSeconds;
            this.activePatchOwners = activePatchOwners
                ?? new Dictionary<FieldPatchCoord, FieldVesselId>();
        }
    }
}
