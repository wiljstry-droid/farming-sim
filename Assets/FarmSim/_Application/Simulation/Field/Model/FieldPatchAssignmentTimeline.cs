using System;
using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data container preserving the full historical assignment timeline for a FieldVessel.
    /// No deletion: ended assignments remain as records with endedUtcSeconds set.
    /// </summary>
    [Serializable]
    public sealed class FieldVesselPatchAssignmentTimeline
    {
        public readonly FieldVesselId vesselId;

        /// <summary>
        /// Full historical record set (order not guaranteed by this type).
        /// </summary>
        public readonly List<FieldPatchAssignmentRecord> records;

        public FieldVesselPatchAssignmentTimeline(FieldVesselId vesselId, List<FieldPatchAssignmentRecord> records)
        {
            this.vesselId = vesselId;
            this.records = records ?? new List<FieldPatchAssignmentRecord>();
        }
    }

    /// <summary>
    /// Pure data container preserving the full historical assignment timeline for a single FieldPatch.
    /// Useful for provenance/history queries later (no behavior in this phase).
    /// </summary>
    [Serializable]
    public sealed class FieldPatchVesselAssignmentTimeline
    {
        public readonly FieldPatchCoord patch;

        /// <summary>
        /// Full historical record set (order not guaranteed by this type).
        /// </summary>
        public readonly List<FieldPatchAssignmentRecord> records;

        public FieldPatchVesselAssignmentTimeline(FieldPatchCoord patch, List<FieldPatchAssignmentRecord> records)
        {
            this.patch = patch;
            this.records = records ?? new List<FieldPatchAssignmentRecord>();
        }
    }
}
