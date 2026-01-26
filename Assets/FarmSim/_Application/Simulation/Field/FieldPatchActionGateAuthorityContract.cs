using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Central gate for permission checks for actions targeting FieldPatches.
    ///
    /// Hard rule (PHASE 17):
    /// - This gate consumes assignment truth via IFieldPatchAssignmentReadOnly only.
    /// - It returns explicit allow/deny decisions with structured reasons.
    /// - It does NOT mutate any assignment data (or anything else).
    /// - In-memory only. No persistence. No UI. No editor tooling.
    /// </summary>
    public interface IFieldPatchActionGateAuthority
    {
        FieldPatchActionGateDecision Decide(in FieldPatchActionGateRequest request);
    }

    /// <summary>
    /// High-level action category targeting a FieldPatch.
    /// This is intentionally coarse for now; we can extend later without breaking consumers.
    /// </summary>
    public enum FieldPatchActionKind
    {
        Unknown = 0,

        /// <summary>
        /// Generic "operate on soil" intent (tillage, treatment application, etc).
        /// </summary>
        SoilOperation = 10,

        /// <summary>
        /// Generic "planting" intent.
        /// </summary>
        Planting = 20,

        /// <summary>
        /// Generic "harvest / remove crop" intent.
        /// </summary>
        Harvest = 30,

        /// <summary>
        /// Generic "inspect / observe" intent. Observation is still gated, but may have different policy later.
        /// </summary>
        Observe = 40
    }

    /// <summary>
    /// Primary decision code for gate outcomes.
    /// Keep this stable; it becomes the contract consumers rely on.
    /// </summary>
    public enum FieldPatchActionGateDecisionCode
    {
        Allowed = 0,

        Denied_InvalidRequest = 10,
        Denied_AssignmentsUnavailable = 20,
        Denied_PatchUnassigned = 30,
        Denied_NotOwner = 40
    }

    /// <summary>
    /// Structured reasons (bit flags) describing why a decision was allowed/denied.
    /// Consumers should prefer these flags over parsing logs or strings.
    /// </summary>
    [System.Flags]
    public enum FieldPatchActionGateReasonFlags
    {
        None = 0,

        InvalidRequest = 1 << 0,
        AssignmentsUnavailable = 1 << 1,
        PatchUnassigned = 1 << 2,
        NotOwner = 1 << 3
    }

    /// <summary>
    /// Pure request for an action gate decision.
    /// </summary>
    public readonly struct FieldPatchActionGateRequest
    {
        public readonly FieldPatchActionKind actionKind;
        public readonly FieldVesselId actingVessel;
        public readonly FieldPatchCoord patch;

        public FieldPatchActionGateRequest(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchCoord patch)
        {
            this.actionKind = actionKind;
            this.actingVessel = actingVessel;
            this.patch = patch;
        }
    }

    /// <summary>
    /// Pure decision result from the action gate.
    /// </summary>
    public readonly struct FieldPatchActionGateDecision
    {
        public readonly bool isAllowed;
        public readonly FieldPatchActionGateDecisionCode code;
        public readonly FieldPatchActionGateReasonFlags reasons;

        /// <summary>
        /// When available, the current active owner of the patch at decision time.
        /// If not known/unavailable, this will be default(FieldVesselId).
        /// </summary>
        public readonly FieldVesselId currentOwner;

        private FieldPatchActionGateDecision(
            bool isAllowed,
            FieldPatchActionGateDecisionCode code,
            FieldPatchActionGateReasonFlags reasons,
            FieldVesselId currentOwner)
        {
            this.isAllowed = isAllowed;
            this.code = code;
            this.reasons = reasons;
            this.currentOwner = currentOwner;
        }

        public static FieldPatchActionGateDecision Allowed(FieldVesselId currentOwner)
            => new FieldPatchActionGateDecision(true, FieldPatchActionGateDecisionCode.Allowed, FieldPatchActionGateReasonFlags.None, currentOwner);

        public static FieldPatchActionGateDecision Denied(FieldPatchActionGateDecisionCode code, FieldPatchActionGateReasonFlags reasons, FieldVesselId currentOwner)
            => new FieldPatchActionGateDecision(false, code, reasons, currentOwner);

        public static FieldPatchActionGateDecision Denied(FieldPatchActionGateDecisionCode code, FieldPatchActionGateReasonFlags reasons)
            => new FieldPatchActionGateDecision(false, code, reasons, default);
    }
}
