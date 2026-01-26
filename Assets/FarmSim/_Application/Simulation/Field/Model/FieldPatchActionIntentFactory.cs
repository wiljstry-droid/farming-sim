using System;
using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// DATA-ONLY convenience construction for FieldPatchActionIntent.
    /// IMPORTANT: This is NOT execution logic. It only fills immutable structs consistently.
    /// </summary>
    public static class FieldPatchActionIntentFactory
    {
        public static FieldPatchActionIntent CreateNoPayload(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds)
        {
            return new FieldPatchActionIntent(
                FieldPatchActionIntentId.NewId(),
                actionKind,
                actingVessel,
                target,
                source,
                requestedUtcSeconds,
                FieldPatchActionPayloadKind.None,
                default,
                default,
                default,
                default);
        }

        public static FieldPatchActionIntent CreateSoilOperation(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds,
            SoilOperationIntentKind kind,
            float intensity)
        {
            var payload = new FieldPatchSoilOperationIntentData(kind, intensity);

            return new FieldPatchActionIntent(
                FieldPatchActionIntentId.NewId(),
                actionKind,
                actingVessel,
                target,
                source,
                requestedUtcSeconds,
                FieldPatchActionPayloadKind.SoilOperation,
                payload,
                default,
                default,
                default);
        }

        public static FieldPatchActionIntent CreatePlanting(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds,
            string speciesId,
            string cultivarId,
            float spacingMeters)
        {
            var payload = new FieldPatchPlantingIntentData(speciesId, cultivarId, spacingMeters);

            return new FieldPatchActionIntent(
                FieldPatchActionIntentId.NewId(),
                actionKind,
                actingVessel,
                target,
                source,
                requestedUtcSeconds,
                FieldPatchActionPayloadKind.Planting,
                default,
                payload,
                default,
                default);
        }

        public static FieldPatchActionIntent CreateHarvest(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds,
            HarvestIntentKind kind)
        {
            var payload = new FieldPatchHarvestIntentData(kind);

            return new FieldPatchActionIntent(
                FieldPatchActionIntentId.NewId(),
                actionKind,
                actingVessel,
                target,
                source,
                requestedUtcSeconds,
                FieldPatchActionPayloadKind.Harvest,
                default,
                default,
                payload,
                default);
        }

        public static FieldPatchActionIntent CreateObserve(
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds,
            ObserveIntentKind kind)
        {
            var payload = new FieldPatchObserveIntentData(kind);

            return new FieldPatchActionIntent(
                FieldPatchActionIntentId.NewId(),
                actionKind,
                actingVessel,
                target,
                source,
                requestedUtcSeconds,
                FieldPatchActionPayloadKind.Observe,
                default,
                default,
                default,
                payload);
        }

        public static FieldPatchActionIntentBatch CreateBatch(
            FieldVesselId actingVessel,
            FieldPatchActionSource source,
            long createdUtcSeconds,
            FieldPatchActionIntent[] intents)
        {
            return new FieldPatchActionIntentBatch(
                FieldPatchActionIntentBatchId.NewId(),
                actingVessel,
                source,
                createdUtcSeconds,
                intents);
        }
    }
}
