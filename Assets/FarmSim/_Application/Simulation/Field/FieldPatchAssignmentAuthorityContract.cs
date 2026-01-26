using System;
using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Narrow contract so other runtime systems can depend on assignment behavior without
    /// depending on the concrete authority type.
    /// In-memory only. No persistence. No UI.
    /// </summary>
    public interface IFieldPatchAssignmentAuthority
    {
        FieldPatchAssignmentIndex Index { get; }

        void Assign(FieldVesselId vesselId, FieldPatchCoord patch, long utcSeconds);
        void Unassign(FieldPatchCoord patch, long utcSeconds);

        bool TryGetActiveOwner(FieldPatchCoord patch, out FieldVesselId vesselId);

        FieldPatchAssignmentSnapshot BuildSnapshot(long utcSeconds);
    }
}
