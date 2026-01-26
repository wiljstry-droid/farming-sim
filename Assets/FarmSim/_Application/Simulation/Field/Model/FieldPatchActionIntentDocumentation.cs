namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 18 — DATA-ONLY DOCUMENTATION ANCHOR
    ///
    /// FieldPatchActionIntent is the canonical representation of "something wants to happen"
    /// to one or more FieldPatches.
    ///
    /// Key properties:
    /// - Immutable / readonly
    /// - Explicitly typed payloads
    /// - Stable identifiers for replay, analytics, batching
    /// - Separate from execution, scheduling, simulation steps
    ///
    /// Typical flow (future phases):
    ///   Intent authored (player/tool/AI)
    ///        ↓
    ///   Shape validation (FieldPatchActionIntentValidator)
    ///        ↓
    ///   Gate bridge derivation (FieldPatchActionIntentGateBridge)
    ///        ↓
    ///   PHASE 17 Action Gate allow/deny
    ///        ↓
    ///   Planning / execution systems (NOT in PHASE 18)
    ///
    /// This file exists to anchor design intent in code without adding behavior.
    /// </summary>
    internal static class FieldPatchActionIntentDocumentation
    {
    }
}
