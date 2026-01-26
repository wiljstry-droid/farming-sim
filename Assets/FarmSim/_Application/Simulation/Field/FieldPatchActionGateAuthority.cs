using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// In-memory gate authority that centralizes permission checks for actions targeting FieldPatches.
    ///
    /// Consumes assignment truth via IFieldPatchAssignmentReadOnly only.
    /// Does not mutate any assignment data (or anything else).
    /// </summary>
    public sealed class FieldPatchActionGateAuthority : IFieldPatchActionGateAuthority
    {
        private readonly IFieldPatchAssignmentReadOnly _assignments;

        public FieldPatchActionGateAuthority(IFieldPatchAssignmentReadOnly assignments)
        {
            _assignments = assignments;
        }

        public FieldPatchActionGateDecision Decide(in FieldPatchActionGateRequest request)
        {
            if (request.actionKind == FieldPatchActionKind.Unknown)
                return FieldPatchActionGateDecision.Denied(
                    FieldPatchActionGateDecisionCode.Denied_InvalidRequest,
                    FieldPatchActionGateReasonFlags.InvalidRequest);

            // FieldVesselId is a readonly struct with a public string 'value' field.
            // Treat null/empty as invalid.
            if (string.IsNullOrEmpty(request.actingVessel.value))
                return FieldPatchActionGateDecision.Denied(
                    FieldPatchActionGateDecisionCode.Denied_InvalidRequest,
                    FieldPatchActionGateReasonFlags.InvalidRequest);

            if (_assignments == null)
                return FieldPatchActionGateDecision.Denied(
                    FieldPatchActionGateDecisionCode.Denied_AssignmentsUnavailable,
                    FieldPatchActionGateReasonFlags.AssignmentsUnavailable);

            FieldVesselId owner;
            if (!_assignments.TryGetActiveOwner(request.patch, out owner))
                return FieldPatchActionGateDecision.Denied(
                    FieldPatchActionGateDecisionCode.Denied_PatchUnassigned,
                    FieldPatchActionGateReasonFlags.PatchUnassigned);

            if (owner != request.actingVessel)
                return FieldPatchActionGateDecision.Denied(
                    FieldPatchActionGateDecisionCode.Denied_NotOwner,
                    FieldPatchActionGateReasonFlags.NotOwner,
                    owner);

            return FieldPatchActionGateDecision.Allowed(owner);
        }
    }
}
