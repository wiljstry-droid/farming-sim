#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick.Diagnostics
{
    /// <summary>
    /// Phase 40: single-per-tick observational proof surface for governed step outcomes.
    /// MUST NOT change simulation behavior. MUST NOT create new execution paths.
    /// </summary>
    public sealed class SimulationTickGovernedStepProofProbe : MonoBehaviour
    {
        [Header("Phase 40: Governed Step Proof (Observational)")]
        [SerializeField] private bool _enabled = true;

        [Tooltip("Only log when a new tick snapshot appears (prevents console spam).")]
        [SerializeField] private bool _logOncePerTick = true;

        [Tooltip("If true, prints each step record id + outcome; otherwise prints only summary counts.")]
        [SerializeField] private bool _includePerStepList = true;

        private long _lastLoggedTickIndex = long.MinValue;

        private static readonly string[] SnapshotRecordPropertyCandidates =
        {
            "Records",
            "ExecutionRecords",
            "StepExecutionRecords",
            "StepRecords"
        };

        private static readonly string[] RecordIdPropertyCandidates =
        {
            "StepId",
            "StepKey",
            "Id",
            "Key",
            "Name"
        };

        private void Update()
        {
            if (!_enabled) return;

            var authority = SimulationTickAuthority.Instance;
            if (authority == null) return;

            var snap = authority.LastExecutionSnapshot;
            if (snap == null) return;

            long tickIndex = snap.TickIndex;

            if (_logOncePerTick && tickIndex == _lastLoggedTickIndex) return;
            _lastLoggedTickIndex = tickIndex;

            if (!TryExtractRecords(snap, out var records))
            {
                Debug.LogWarning($"[GovProof] tick={tickIndex} (no accessible records on snapshot)");
                return;
            }

            int total = 0;
            int succeeded = 0;
            int denied = 0;
            int failed = 0;
            int other = 0;

            for (int i = 0; i < records.Count; i++)
            {
                total++;

                var outcome = records[i].Outcome;

                // We stay observational: classify by enum name (no coupling to specific numeric values).
                var name = outcome.ToString();

                if (name.IndexOf("Success", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    name.IndexOf("Succeeded", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    succeeded++;
                }
                else if (name.IndexOf("Denied", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         name.IndexOf("Govern", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    denied++;
                }
                else if (name.IndexOf("Fail", StringComparison.OrdinalIgnoreCase) >= 0 ||
                         name.IndexOf("Error", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    failed++;
                }
                else
                {
                    other++;
                }
            }

            var sb = new StringBuilder(512);
            sb.Append("[GovProof] tick=").Append(tickIndex)
              .Append(" total=").Append(total)
              .Append(" success=").Append(succeeded)
              .Append(" denied=").Append(denied)
              .Append(" failed=").Append(failed);

            if (other > 0) sb.Append(" other=").Append(other);

            if (_includePerStepList && records.Count > 0)
            {
                sb.Append(" | ");

                for (int i = 0; i < records.Count; i++)
                {
                    var rec = records[i];
                    string id = TryExtractRecordId(rec) ?? "<?>";

                    sb.Append(id).Append(":").Append(rec.Outcome);

                    if (i < records.Count - 1) sb.Append(", ");
                }
            }

            Debug.Log(sb.ToString());
        }

        private static bool TryExtractRecords(
            SimulationTickExecutionSnapshot snapshot,
            out IReadOnlyList<SimulationStepExecutionRecord> records)
        {
            // We intentionally use reflection so Phase 40 does NOT depend on snapshot API naming,
            // and so we can remain observational without changing model contracts.
            var t = snapshot.GetType();

            for (int i = 0; i < SnapshotRecordPropertyCandidates.Length; i++)
            {
                var prop = t.GetProperty(
                    SnapshotRecordPropertyCandidates[i],
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (prop == null) continue;

                object? value = prop.GetValue(snapshot);
                if (value == null) continue;

                if (value is IReadOnlyList<SimulationStepExecutionRecord> roList)
                {
                    records = roList;
                    return true;
                }

                if (value is IList list)
                {
                    // Attempt to copy to a typed list.
                    var typed = new List<SimulationStepExecutionRecord>(list.Count);
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j] is SimulationStepExecutionRecord rec) typed.Add(rec);
                    }

                    records = typed;
                    return true;
                }

                if (value is IEnumerable enumerable)
                {
                    var typed = new List<SimulationStepExecutionRecord>();
                    foreach (var item in enumerable)
                    {
                        if (item is SimulationStepExecutionRecord rec) typed.Add(rec);
                    }

                    if (typed.Count > 0)
                    {
                        records = typed;
                        return true;
                    }
                }
            }

            records = Array.Empty<SimulationStepExecutionRecord>();
            return false;
        }

        private static string? TryExtractRecordId(SimulationStepExecutionRecord record)
        {
            var t = record.GetType();

            for (int i = 0; i < RecordIdPropertyCandidates.Length; i++)
            {
                var prop = t.GetProperty(
                    RecordIdPropertyCandidates[i],
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (prop == null) continue;

                object? value = prop.GetValue(record);
                if (value == null) continue;

                return value as string ?? value.ToString();
            }

            return null;
        }
    }
}
