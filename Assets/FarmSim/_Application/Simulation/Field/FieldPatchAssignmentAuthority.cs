using System;
using System.Collections.Generic;
using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// In-memory runtime authority that mutates FieldPatch assignment data (index/timelines/records).
    /// No persistence, no UI, no simulation-step integration.
    /// </summary>
    public sealed class FieldPatchAssignmentAuthority : IFieldPatchAssignmentAuthority
    {
        private readonly FieldPatchAssignmentIndex _index;

        public FieldPatchAssignmentAuthority(FieldPatchAssignmentIndex index)
        {
            _index = index ?? FieldPatchAssignmentDataFactory.CreateEmptyIndex();
        }

        public FieldPatchAssignmentIndex Index => _index;

        /// <summary>
        /// Assigns a patch to a vessel at the given UTC seconds.
        /// Enforces: a patch has at most one active owner at a time.
        /// History is preserved by ending prior active records.
        /// </summary>
        public void Assign(FieldVesselId vesselId, FieldPatchCoord patch, long utcSeconds)
        {
            // 1) End any active assignment for this patch (if owned by someone else OR same owner).
            EndActiveForPatch(patch, utcSeconds);

            // 2) Create a new active record.
            var record = new FieldPatchAssignmentRecord(
                vesselId,
                patch,
                utcSeconds,
                endedUtcSeconds: null);

            // 3) Append to both timelines.
            GetOrCreateVesselTimeline(vesselId).records.Add(record);
            GetOrCreatePatchTimeline(patch).records.Add(record);
        }

        /// <summary>
        /// Ends the currently active assignment for a patch (if any) at the given UTC seconds.
        /// </summary>
        public void Unassign(FieldPatchCoord patch, long utcSeconds)
        {
            EndActiveForPatch(patch, utcSeconds);
        }

        /// <summary>
        /// Returns true if the patch has an active owner.
        /// </summary>
        public bool TryGetActiveOwner(FieldPatchCoord patch, out FieldVesselId vesselId)
        {
            vesselId = default;

            if (!_index.byPatch.TryGetValue(patch, out var patchTimeline))
                return false;

            var records = patchTimeline.records;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                if (records[i].IsActive)
                {
                    vesselId = records[i].vesselId;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Builds a pure-data snapshot of active assignments at the given UTC seconds.
        /// Since records are stored as created/ended timestamps, "active at time" means:
        /// created <= t and (ended is null OR ended > t).
        /// </summary>
        public FieldPatchAssignmentSnapshot BuildSnapshot(long utcSeconds)
        {
            var active = new Dictionary<FieldPatchCoord, FieldVesselId>();

            foreach (var kvp in _index.byPatch)
            {
                var patch = kvp.Key;
                var timeline = kvp.Value;

                // Find most recent record that is active at utcSeconds.
                var records = timeline.records;
                for (int i = records.Count - 1; i >= 0; i--)
                {
                    var r = records[i];

                    if (r.createdUtcSeconds > utcSeconds)
                        continue;

                    if (!r.endedUtcSeconds.HasValue || r.endedUtcSeconds.Value > utcSeconds)
                    {
                        active[patch] = r.vesselId;
                        break;
                    }
                }
            }

            return new FieldPatchAssignmentSnapshot(utcSeconds, active);
        }

        private FieldVesselPatchAssignmentTimeline GetOrCreateVesselTimeline(FieldVesselId vesselId)
        {
            if (_index.byVessel.TryGetValue(vesselId, out var timeline))
                return timeline;

            timeline = FieldPatchAssignmentDataFactory.CreateEmptyTimeline(vesselId);
            _index.byVessel[vesselId] = timeline;
            return timeline;
        }

        private FieldPatchVesselAssignmentTimeline GetOrCreatePatchTimeline(FieldPatchCoord patch)
        {
            if (_index.byPatch.TryGetValue(patch, out var timeline))
                return timeline;

            timeline = FieldPatchAssignmentDataFactory.CreateEmptyTimeline(patch);
            _index.byPatch[patch] = timeline;
            return timeline;
        }

        private void EndActiveForPatch(FieldPatchCoord patch, long utcSeconds)
        {
            if (!_index.byPatch.TryGetValue(patch, out var patchTimeline))
                return;

            // Find current active record for this patch (if any).
            FieldPatchAssignmentRecord? active = null;
            var patchRecords = patchTimeline.records;

            for (int i = patchRecords.Count - 1; i >= 0; i--)
            {
                if (patchRecords[i].IsActive)
                {
                    active = patchRecords[i];
                    break;
                }
            }

            if (!active.HasValue)
                return;

            var activeRecord = active.Value;

            // Safety: do not allow ending earlier than creation.
            if (utcSeconds < activeRecord.createdUtcSeconds)
            {
                utcSeconds = activeRecord.createdUtcSeconds;
            }

            // Replace the active record with an ended record in BOTH timelines.
            var endedRecord = new FieldPatchAssignmentRecord(
                activeRecord.vesselId,
                activeRecord.patch,
                activeRecord.createdUtcSeconds,
                endedUtcSeconds: utcSeconds);

            ReplaceRecordInPatchTimeline(patchTimeline, activeRecord, endedRecord);

            if (_index.byVessel.TryGetValue(activeRecord.vesselId, out var vesselTimeline))
            {
                ReplaceRecordInVesselTimeline(vesselTimeline, activeRecord, endedRecord);
            }
        }

        private static void ReplaceRecordInPatchTimeline(
            FieldPatchVesselAssignmentTimeline timeline,
            FieldPatchAssignmentRecord from,
            FieldPatchAssignmentRecord to)
        {
            var records = timeline.records;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                if (records[i].Equals(from))
                {
                    records[i] = to;
                    return;
                }
            }
        }

        private static void ReplaceRecordInVesselTimeline(
            FieldVesselPatchAssignmentTimeline timeline,
            FieldPatchAssignmentRecord from,
            FieldPatchAssignmentRecord to)
        {
            var records = timeline.records;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                if (records[i].Equals(from))
                {
                    records[i] = to;
                    return;
                }
            }
        }
    }
}
