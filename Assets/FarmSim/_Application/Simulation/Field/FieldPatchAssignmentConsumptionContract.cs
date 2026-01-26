using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Read-only consumption contract for FieldPatch assignment truth.
    /// This is the ONLY thing most runtime systems should depend on.
    ///
    /// Hard rule: no mutation methods live here.
    /// In-memory only. No persistence. No UI.
    /// </summary>
    public interface IFieldPatchAssignmentReadOnly
    {
        /// <summary>
        /// Returns true if the patch has an active owner "now" (active record with no end).
        /// </summary>
        bool TryGetActiveOwner(FieldPatchCoord patch, out FieldVesselId vesselId);

        /// <summary>
        /// Builds a pure-data snapshot of active assignments at the given UTC seconds.
        /// (Uses created/ended timestamps stored in records.)
        /// </summary>
        FieldPatchAssignmentSnapshot BuildSnapshot(long utcSeconds);
    }

    /// <summary>
    /// Eligibility result for "who may act on which FieldPatch" decisions.
    /// Designed to be safe + explicit for consumers.
    /// </summary>
    public readonly struct FieldPatchAssignmentEligibility
    {
        public readonly bool isEligible;
        public readonly FieldPatchAssignmentEligibilityReason reason;
        public readonly FieldVesselId currentOwner;

        private FieldPatchAssignmentEligibility(
            bool isEligible,
            FieldPatchAssignmentEligibilityReason reason,
            FieldVesselId currentOwner)
        {
            this.isEligible = isEligible;
            this.reason = reason;
            this.currentOwner = currentOwner;
        }

        public static FieldPatchAssignmentEligibility Eligible()
            => new FieldPatchAssignmentEligibility(true, FieldPatchAssignmentEligibilityReason.Eligible, default);

        public static FieldPatchAssignmentEligibility NotAssigned()
            => new FieldPatchAssignmentEligibility(false, FieldPatchAssignmentEligibilityReason.NotAssigned, default);

        public static FieldPatchAssignmentEligibility OwnedByOther(FieldVesselId owner)
            => new FieldPatchAssignmentEligibility(false, FieldPatchAssignmentEligibilityReason.OwnedByOther, owner);
    }

    public enum FieldPatchAssignmentEligibilityReason
    {
        Eligible = 0,
        NotAssigned = 10,
        OwnedByOther = 20
    }

    /// <summary>
    /// Small, deterministic helper used by runtime systems to validate actions against assignment truth.
    /// No mutation. No Unity dependencies.
    /// </summary>
    public static class FieldPatchAssignmentConsumption
    {
        /// <summary>
        /// Checks if the given vessel may act on the patch, based on current active ownership.
        ///
        /// Policy (PHASE 16):
        /// - If patch is unassigned: NOT eligible (guardrail; prevents "acting on nobody's patch").
        /// - If patch is assigned to the same vessel: eligible.
        /// - If patch is assigned to another vessel: NOT eligible.
        /// </summary>
        public static FieldPatchAssignmentEligibility CheckEligibility(
            IFieldPatchAssignmentReadOnly assignments,
            FieldVesselId actingVessel,
            FieldPatchCoord patch)
        {
            if (assignments == null)
                return FieldPatchAssignmentEligibility.NotAssigned();

            if (!assignments.TryGetActiveOwner(patch, out var owner))
                return FieldPatchAssignmentEligibility.NotAssigned();

            if (owner.Equals(actingVessel))
                return FieldPatchAssignmentEligibility.Eligible();

            return FieldPatchAssignmentEligibility.OwnedByOther(owner);
        }

        /// <summary>
        /// Convenience boolean: true only if the patch is actively owned by the acting vessel.
        /// </summary>
        public static bool IsOwnedBy(
            IFieldPatchAssignmentReadOnly assignments,
            FieldVesselId actingVessel,
            FieldPatchCoord patch)
        {
            if (assignments == null)
                return false;

            return assignments.TryGetActiveOwner(patch, out var owner) && owner.Equals(actingVessel);
        }
    }
}
