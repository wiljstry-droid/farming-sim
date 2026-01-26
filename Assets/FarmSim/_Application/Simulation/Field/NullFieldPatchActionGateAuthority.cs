using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Null-object implementation of the FieldPatch action gate.
    /// Returns a deterministic denial that indicates the gate is unavailable/unbound.
    ///
    /// In-memory only. No mutation. No persistence. No UI. No editor tooling.
    /// </summary>
    public sealed class NullFieldPatchActionGateAuthority : IFieldPatchActionGateAuthority
    {
        public static readonly NullFieldPatchActionGateAuthority Instance = new NullFieldPatchActionGateAuthority();

        private NullFieldPatchActionGateAuthority() { }

        public FieldPatchActionGateDecision Decide(in FieldPatchActionGateRequest request)
        {
            // Always deny with a stable reason indicating the gate is not currently backed by assignment truth.
            return FieldPatchActionGateDecision.Denied(
                FieldPatchActionGateDecisionCode.Denied_AssignmentsUnavailable,
                FieldPatchActionGateReasonFlags.AssignmentsUnavailable);
        }
    }
}
