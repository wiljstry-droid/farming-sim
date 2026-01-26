using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// DATA ONLY: lightweight validation result for action intents.
    /// This does NOT execute, schedule, or mutate anything.
    /// Intended for upstream systems (UI/tools/AI) to preflight intent shape
    /// before asking the PHASE 17 Action Gate for allow/deny.
    /// </summary>
    public readonly struct FieldPatchActionIntentValidationResult
    {
        public readonly FieldPatchActionIntentValidationCode code;

        public readonly FieldPatchActionIntentValidationFlags flags;

        public FieldPatchActionIntentValidationResult(
            FieldPatchActionIntentValidationCode code,
            FieldPatchActionIntentValidationFlags flags)
        {
            this.code = code;
            this.flags = flags;
        }

        public static FieldPatchActionIntentValidationResult Ok()
            => new FieldPatchActionIntentValidationResult(
                FieldPatchActionIntentValidationCode.Ok,
                FieldPatchActionIntentValidationFlags.None);
    }

    public enum FieldPatchActionIntentValidationCode
    {
        Ok = 0,

        InvalidIntentId = 10,
        InvalidActionKind = 20,
        InvalidActingVessel = 30,
        InvalidTarget = 40,
        InvalidTimestamp = 50,

        PayloadKindMismatch = 60,
        MissingPayload = 70,
        InvalidPayload = 80
    }

    [System.Flags]
    public enum FieldPatchActionIntentValidationFlags
    {
        None = 0,

        TargetUnknown = 1 << 0,
        TargetEmptyPatchList = 1 << 1,

        SourceUnknown = 1 << 2,

        PayloadNone = 1 << 3,
        PayloadSoil = 1 << 4,
        PayloadPlanting = 1 << 5,
        PayloadHarvest = 1 << 6,
        PayloadObserve = 1 << 7
    }
}
