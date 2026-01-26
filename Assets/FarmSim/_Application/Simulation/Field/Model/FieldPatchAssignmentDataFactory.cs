using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data factory helpers for creating assignment data containers.
    /// No simulation behavior; just constructors grouped for convenience.
    /// </summary>
    public static class FieldPatchAssignmentDataFactory
    {
        public static FieldPatchAssignmentIndex CreateEmptyIndex()
        {
            return new FieldPatchAssignmentIndex(
                new Dictionary<FieldVesselId, FieldVesselPatchAssignmentTimeline>(),
                new Dictionary<FieldPatchCoord, FieldPatchVesselAssignmentTimeline>());
        }

        public static FieldVesselPatchAssignmentTimeline CreateEmptyTimeline(FieldVesselId vesselId)
        {
            return new FieldVesselPatchAssignmentTimeline(vesselId, new List<FieldPatchAssignmentRecord>());
        }

        public static FieldPatchVesselAssignmentTimeline CreateEmptyTimeline(FieldPatchCoord patch)
        {
            return new FieldPatchVesselAssignmentTimeline(patch, new List<FieldPatchAssignmentRecord>());
        }
    }
}
