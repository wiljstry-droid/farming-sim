using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// DATA ONLY: minimal bridge view of an intent for consumption by the PHASE 17 Action Gate.
    /// This is intentionally small and stable: it carries only what the gate needs to reason.
    /// Future systems can derive this from richer intents without changing the gate.
    /// </summary>
    public readonly struct FieldPatchActionIntentGateBridge
    {
        public readonly FieldPatchActionKind actionKind;

        public readonly FieldVesselId actingVessel;

        public readonly FieldPatchActionTarget target;

        public FieldPatchActionIntentGateBridge(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target)
        {
            this.actionKind = actionKind;
            this.actingVessel = actingVessel;
            this.target = target;
        }

        public static FieldPatchActionIntentGateBridge FromIntent(in FieldPatchActionIntent intent)
            => new FieldPatchActionIntentGateBridge(intent.actionKind, intent.actingVessel, intent.target);
    }
}
