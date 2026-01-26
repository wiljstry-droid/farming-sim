using System;
using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data index bundling vessel-centric and patch-centric assignment timelines.
    /// No mutation rules, no invariants enforced in this phase.
    /// </summary>
    [Serializable]
    public sealed class FieldPatchAssignmentIndex
    {
        /// <summary>
        /// Vessel -> patch assignment history.
        /// </summary>
        public readonly Dictionary<FieldVesselId, FieldVesselPatchAssignmentTimeline> byVessel;

        /// <summary>
        /// Patch -> vessel assignment history.
        /// </summary>
        public readonly Dictionary<FieldPatchCoord, FieldPatchVesselAssignmentTimeline> byPatch;

        public FieldPatchAssignmentIndex(
            Dictionary<FieldVesselId, FieldVesselPatchAssignmentTimeline> byVessel,
            Dictionary<FieldPatchCoord, FieldPatchVesselAssignmentTimeline> byPatch)
        {
            this.byVessel = byVessel ?? new Dictionary<FieldVesselId, FieldVesselPatchAssignmentTimeline>();
            this.byPatch = byPatch ?? new Dictionary<FieldPatchCoord, FieldPatchVesselAssignmentTimeline>();
        }
    }
}
