using System;
using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// SHAPE-ONLY validation for FieldPatchActionIntent.
    /// DATA-ONLY PHASE 18 allowance:
    /// - No mutation
    /// - No gate bypass (this does NOT allow/deny execution)
    /// - No scheduling/queues
    /// This exists solely to validate intent data completeness/consistency.
    /// </summary>
    public static class FieldPatchActionIntentValidator
    {
        public static FieldPatchActionIntentValidationResult Validate(in FieldPatchActionIntent intent)
        {
            var flags = FieldPatchActionIntentValidationFlags.None;

            if (intent.intentId.value == Guid.Empty)
                return new FieldPatchActionIntentValidationResult(
                    FieldPatchActionIntentValidationCode.InvalidIntentId, flags);

            if ((int)intent.actionKind == 0)
                return new FieldPatchActionIntentValidationResult(
                    FieldPatchActionIntentValidationCode.InvalidActionKind, flags);

            // Note: FieldVesselId / FieldPatchCoord are authoritative types elsewhere.
            // We can only do shallow/default checks here without execution logic.
            if (intent.actingVessel.Equals(default(FieldVesselId)))
                return new FieldPatchActionIntentValidationResult(
                    FieldPatchActionIntentValidationCode.InvalidActingVessel, flags);

            if (intent.requestedUtcSeconds <= 0)
                return new FieldPatchActionIntentValidationResult(
                    FieldPatchActionIntentValidationCode.InvalidTimestamp, flags);

            // Target shape checks.
            if (intent.target.kind == FieldPatchActionTargetKind.Unknown)
                flags |= FieldPatchActionIntentValidationFlags.TargetUnknown;

            if (intent.target.kind == FieldPatchActionTargetKind.PatchList)
            {
                if (intent.target.patches == null || intent.target.patches.Length == 0)
                    flags |= FieldPatchActionIntentValidationFlags.TargetEmptyPatchList;
            }

            if (intent.target.kind == FieldPatchActionTargetKind.Unknown)
            {
                return new FieldPatchActionIntentValidationResult(
                    FieldPatchActionIntentValidationCode.InvalidTarget, flags);
            }

            // Source (optional but flagged).
            if (intent.source.kind == FieldPatchActionSourceKind.Unknown)
                flags |= FieldPatchActionIntentValidationFlags.SourceUnknown;

            // Payload consistency.
            switch (intent.payloadKind)
            {
                case FieldPatchActionPayloadKind.None:
                    flags |= FieldPatchActionIntentValidationFlags.PayloadNone;
                    break;

                case FieldPatchActionPayloadKind.SoilOperation:
                    flags |= FieldPatchActionIntentValidationFlags.PayloadSoil;
                    if (intent.soilOperation.kind == SoilOperationIntentKind.Unknown)
                        return new FieldPatchActionIntentValidationResult(
                            FieldPatchActionIntentValidationCode.InvalidPayload, flags);
                    break;

                case FieldPatchActionPayloadKind.Planting:
                    flags |= FieldPatchActionIntentValidationFlags.PayloadPlanting;
                    if (string.IsNullOrEmpty(intent.planting.speciesId))
                        return new FieldPatchActionIntentValidationResult(
                            FieldPatchActionIntentValidationCode.InvalidPayload, flags);
                    break;

                case FieldPatchActionPayloadKind.Harvest:
                    flags |= FieldPatchActionIntentValidationFlags.PayloadHarvest;
                    if (intent.harvest.kind == HarvestIntentKind.Unknown)
                        return new FieldPatchActionIntentValidationResult(
                            FieldPatchActionIntentValidationCode.InvalidPayload, flags);
                    break;

                case FieldPatchActionPayloadKind.Observe:
                    flags |= FieldPatchActionIntentValidationFlags.PayloadObserve;
                    if (intent.observe.kind == ObserveIntentKind.Unknown)
                        return new FieldPatchActionIntentValidationResult(
                            FieldPatchActionIntentValidationCode.InvalidPayload, flags);
                    break;

                default:
                    return new FieldPatchActionIntentValidationResult(
                        FieldPatchActionIntentValidationCode.PayloadKindMismatch, flags);
            }

            return new FieldPatchActionIntentValidationResult(
                FieldPatchActionIntentValidationCode.Ok, flags);
        }
    }
}
