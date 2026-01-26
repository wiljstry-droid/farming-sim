using System;
using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Immutable batch container for action intents.
    /// DATA ONLY (PHASE 18): no execution logic, no scheduling/queues.
    /// Enables planning, replay, analytics, and deterministic grouping.
    /// </summary>
    public readonly struct FieldPatchActionIntentBatch
    {
        public readonly FieldPatchActionIntentBatchId batchId;

        /// <summary>Who authored/issued the batch (often same across intents).</summary>
        public readonly FieldVesselId actingVessel;

        /// <summary>Batch origin metadata.</summary>
        public readonly FieldPatchActionSource source;

        /// <summary>Batch created time in UTC seconds.</summary>
        public readonly long createdUtcSeconds;

        /// <summary>Ordered intents in this batch (defensively copied).</summary>
        public readonly FieldPatchActionIntent[] intents;

        public FieldPatchActionIntentBatch(
            FieldPatchActionIntentBatchId batchId,
            FieldVesselId actingVessel,
            FieldPatchActionSource source,
            long createdUtcSeconds,
            FieldPatchActionIntent[] intents)
        {
            this.batchId = batchId;
            this.actingVessel = actingVessel;
            this.source = source;
            this.createdUtcSeconds = createdUtcSeconds;

            if (intents == null || intents.Length == 0)
            {
                this.intents = Array.Empty<FieldPatchActionIntent>();
                return;
            }

            var copy = new FieldPatchActionIntent[intents.Length];
            Array.Copy(intents, copy, intents.Length);
            this.intents = copy;
        }
    }

    /// <summary>Stable identifier for intent batches.</summary>
    public readonly struct FieldPatchActionIntentBatchId
    {
        public readonly Guid value;

        public FieldPatchActionIntentBatchId(Guid value)
        {
            this.value = value;
        }

        public static FieldPatchActionIntentBatchId NewId()
            => new FieldPatchActionIntentBatchId(Guid.NewGuid());
    }
}
