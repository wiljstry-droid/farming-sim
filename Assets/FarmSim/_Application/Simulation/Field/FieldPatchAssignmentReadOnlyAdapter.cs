using System;
using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Thin adapter that exposes read-only assignment truth from the (write-capable) authority contract.
    /// This prevents consumers from depending on mutation methods while still letting us reuse the same backing truth.
    ///
    /// In-memory only. No persistence. No UI.
    /// </summary>
    public sealed class FieldPatchAssignmentReadOnlyAdapter : IFieldPatchAssignmentReadOnly
    {
        private readonly IFieldPatchAssignmentAuthority _authority;

        public FieldPatchAssignmentReadOnlyAdapter(IFieldPatchAssignmentAuthority authority)
        {
            _authority = authority ?? throw new ArgumentNullException(nameof(authority));
        }

        public bool TryGetActiveOwner(FieldPatchCoord patch, out FieldVesselId vesselId)
        {
            return _authority.TryGetActiveOwner(patch, out vesselId);
        }

        public FieldPatchAssignmentSnapshot BuildSnapshot(long utcSeconds)
        {
            return _authority.BuildSnapshot(utcSeconds);
        }
    }
}
