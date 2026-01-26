using System;
using FarmSim.Application.Simulation.Field;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Canonical, immutable representation of an intent to perform an action targeting FieldPatches.
    /// DATA ONLY (PHASE 18): no execution logic, no queues, no mutation.
    /// </summary>
    public readonly struct FieldPatchActionIntent
    {
        public readonly FieldPatchActionIntentId intentId;

        /// <summary>High-level action category (shared with the Action Gate domain).</summary>
        public readonly FieldPatchActionKind actionKind;

        /// <summary>Who is attempting the action (eg. a vessel/tool/agent).</summary>
        public readonly FieldVesselId actingVessel;

        /// <summary>What patch(es) the action targets.</summary>
        public readonly FieldPatchActionTarget target;

        /// <summary>Where this intent came from (player/tool/AI/system).</summary>
        public readonly FieldPatchActionSource source;

        /// <summary>
        /// When the intent was authored/raised, in UTC seconds.
        /// (Scheduling/planning can extend this later without changing the core model.)
        /// </summary>
        public readonly long requestedUtcSeconds;

        /// <summary>Optional typed payload category.</summary>
        public readonly FieldPatchActionPayloadKind payloadKind;

        /// <summary>Payload (valid only when payloadKind == SoilOperation).</summary>
        public readonly FieldPatchSoilOperationIntentData soilOperation;

        /// <summary>Payload (valid only when payloadKind == Planting).</summary>
        public readonly FieldPatchPlantingIntentData planting;

        /// <summary>Payload (valid only when payloadKind == Harvest).</summary>
        public readonly FieldPatchHarvestIntentData harvest;

        /// <summary>Payload (valid only when payloadKind == Observe).</summary>
        public readonly FieldPatchObserveIntentData observe;

        public FieldPatchActionIntent(
            FieldPatchActionIntentId intentId,
            FieldPatchActionKind actionKind,
            FieldVesselId actingVessel,
            FieldPatchActionTarget target,
            FieldPatchActionSource source,
            long requestedUtcSeconds,
            FieldPatchActionPayloadKind payloadKind,
            in FieldPatchSoilOperationIntentData soilOperation,
            in FieldPatchPlantingIntentData planting,
            in FieldPatchHarvestIntentData harvest,
            in FieldPatchObserveIntentData observe)
        {
            this.intentId = intentId;
            this.actionKind = actionKind;
            this.actingVessel = actingVessel;
            this.target = target;
            this.source = source;
            this.requestedUtcSeconds = requestedUtcSeconds;

            this.payloadKind = payloadKind;
            this.soilOperation = soilOperation;
            this.planting = planting;
            this.harvest = harvest;
            this.observe = observe;
        }
    }

    /// <summary>Stable identifier for intents (supports batching, replay, analytics).</summary>
    public readonly struct FieldPatchActionIntentId
    {
        public readonly Guid value;

        public FieldPatchActionIntentId(Guid value)
        {
            this.value = value;
        }

        public static FieldPatchActionIntentId NewId()
            => new FieldPatchActionIntentId(Guid.NewGuid());
    }

    public enum FieldPatchActionTargetKind
    {
        Unknown = 0,
        SinglePatch = 10,
        PatchList = 20
        // Future-proofing: Region, Polygon, Query, etc. can be added later.
    }

    /// <summary>
    /// Target selector for a FieldPatch action. Immutable container.
    /// NOTE: Patch arrays are defensively copied in constructors to preserve immutability.
    /// </summary>
    public readonly struct FieldPatchActionTarget
    {
        public readonly FieldPatchActionTargetKind kind;

        public readonly FieldPatchCoord singlePatch;

        public readonly FieldPatchCoord[] patches;

        private FieldPatchActionTarget(
            FieldPatchActionTargetKind kind,
            FieldPatchCoord singlePatch,
            FieldPatchCoord[] patches)
        {
            this.kind = kind;
            this.singlePatch = singlePatch;
            this.patches = patches;
        }

        public static FieldPatchActionTarget Single(in FieldPatchCoord patch)
            => new FieldPatchActionTarget(FieldPatchActionTargetKind.SinglePatch, patch, null);

        public static FieldPatchActionTarget PatchList(FieldPatchCoord[] patchList)
        {
            if (patchList == null || patchList.Length == 0)
                return new FieldPatchActionTarget(FieldPatchActionTargetKind.Unknown, default, null);

            var copy = new FieldPatchCoord[patchList.Length];
            Array.Copy(patchList, copy, patchList.Length);

            return new FieldPatchActionTarget(FieldPatchActionTargetKind.PatchList, default, copy);
        }
    }

    public enum FieldPatchActionSourceKind
    {
        Unknown = 0,
        Player = 10,
        Tool = 20,
        AI = 30,
        System = 40
    }

    /// <summary>Origin metadata (data only).</summary>
    public readonly struct FieldPatchActionSource
    {
        public readonly FieldPatchActionSourceKind kind;

        /// <summary>
        /// Optional identifier for the origin (eg. tool id, AI plan id, UI action id).
        /// Null/empty is allowed.
        /// </summary>
        public readonly string sourceId;

        public FieldPatchActionSource(FieldPatchActionSourceKind kind, string sourceId)
        {
            this.kind = kind;
            this.sourceId = sourceId;
        }

        public static FieldPatchActionSource Unknown()
            => new FieldPatchActionSource(FieldPatchActionSourceKind.Unknown, null);
    }

    public enum FieldPatchActionPayloadKind
    {
        None = 0,
        SoilOperation = 10,
        Planting = 20,
        Harvest = 30,
        Observe = 40
    }

    // --- Typed payloads (DATA ONLY) ---

    public enum SoilOperationIntentKind
    {
        Unknown = 0,
        Tillage = 10,
        TreatmentApply = 20
    }

    public readonly struct FieldPatchSoilOperationIntentData
    {
        public readonly SoilOperationIntentKind kind;

        /// <summary>Optional intensity scalar (interpretation belongs to future execution/planning).</summary>
        public readonly float intensity;

        public FieldPatchSoilOperationIntentData(SoilOperationIntentKind kind, float intensity)
        {
            this.kind = kind;
            this.intensity = intensity;
        }
    }

    public readonly struct FieldPatchPlantingIntentData
    {
        /// <summary>Species/crop identifier (string id; resolution is a future system).</summary>
        public readonly string speciesId;

        /// <summary>Cultivar/variety identifier (string id; resolution is a future system).</summary>
        public readonly string cultivarId;

        /// <summary>Optional spacing in meters (0 means unspecified).</summary>
        public readonly float spacingMeters;

        public FieldPatchPlantingIntentData(string speciesId, string cultivarId, float spacingMeters)
        {
            this.speciesId = speciesId;
            this.cultivarId = cultivarId;
            this.spacingMeters = spacingMeters;
        }
    }

    public enum HarvestIntentKind
    {
        Unknown = 0,
        HarvestYield = 10,
        RemoveCrop = 20
    }

    public readonly struct FieldPatchHarvestIntentData
    {
        public readonly HarvestIntentKind kind;

        public FieldPatchHarvestIntentData(HarvestIntentKind kind)
        {
            this.kind = kind;
        }
    }

    public enum ObserveIntentKind
    {
        Unknown = 0,
        Inspect = 10,
        Sample = 20
    }

    public readonly struct FieldPatchObserveIntentData
    {
        public readonly ObserveIntentKind kind;

        public FieldPatchObserveIntentData(ObserveIntentKind kind)
        {
            this.kind = kind;
        }
    }
}
