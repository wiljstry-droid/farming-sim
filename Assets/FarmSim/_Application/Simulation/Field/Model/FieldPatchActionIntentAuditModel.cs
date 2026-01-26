using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 19 (DATA ONLY)
    /// In-memory, read-only snapshots for action intent history / auditing.
    /// Supports replay, analytics, debugging.
    /// No persistence, no execution, no MonoBehaviours.
    /// </summary>
    public static class FieldPatchActionIntentAuditModel
    {
        /// <summary>
        /// Immutable snapshot of an intent and its reasoning artifacts at time of recording.
        /// This is intentionally "by reference" for intent/payload models defined earlier,
        /// but wrapped in an immutable snapshot container.
        /// </summary>
        public sealed class IntentAuditSnapshot
        {
            public Guid AuditId { get; }
            public double RecordedSimSeconds { get; }

            /// <summary>
            /// The original action intent (PHASE 18).
            /// </summary>
            public object ActionIntent { get; }

            /// <summary>
            /// Optional planning context (PHASE 19).
            /// </summary>
            public FieldPatchActionPlanningContextModel.PlanningContext Planning { get; }

            /// <summary>
            /// Optional gate evaluation result (PHASE 19).
            /// </summary>
            public FieldPatchActionGateEvaluationResultModel.EvaluationResult GateEvaluation { get; }

            /// <summary>
            /// Optional short note for debugging/analytics.
            /// </summary>
            public string Note { get; }

            internal IntentAuditSnapshot(
                Guid auditId,
                double recordedSimSeconds,
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planning,
                FieldPatchActionGateEvaluationResultModel.EvaluationResult gateEvaluation,
                string note)
            {
                AuditId = auditId;
                RecordedSimSeconds = recordedSimSeconds;
                ActionIntent = actionIntent;
                Planning = planning;
                GateEvaluation = gateEvaluation;
                Note = note ?? "";
            }
        }

        /// <summary>
        /// Read-only view over a set of snapshots.
        /// </summary>
        public sealed class IntentAuditReadOnly
        {
            private readonly ReadOnlyCollection<IntentAuditSnapshot> _snapshots;

            public ReadOnlyCollection<IntentAuditSnapshot> Snapshots => _snapshots;

            internal IntentAuditReadOnly(List<IntentAuditSnapshot> snapshots)
            {
                if (snapshots == null || snapshots.Count == 0)
                {
                    _snapshots = new ReadOnlyCollection<IntentAuditSnapshot>(Array.Empty<IntentAuditSnapshot>());
                    return;
                }

                // Copy list to prevent external mutation.
                var copy = new IntentAuditSnapshot[snapshots.Count];
                for (int i = 0; i < snapshots.Count; i++)
                    copy[i] = snapshots[i];

                _snapshots = new ReadOnlyCollection<IntentAuditSnapshot>(copy);
            }
        }

        /// <summary>
        /// In-memory audit buffer (mutable internally, exposes only read-only snapshots).
        /// This is NOT a scheduler/queue; it's a passive history recorder.
        /// </summary>
        public sealed class IntentAuditBuffer
        {
            private readonly List<IntentAuditSnapshot> _history;

            public int Count => _history.Count;

            public IntentAuditBuffer(int initialCapacity = 64)
            {
                if (initialCapacity < 0) initialCapacity = 0;
                _history = new List<IntentAuditSnapshot>(initialCapacity);
            }

            public void Clear()
            {
                _history.Clear();
            }

            public IntentAuditSnapshot Record(
                double recordedSimSeconds,
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planning = null,
                FieldPatchActionGateEvaluationResultModel.EvaluationResult gateEvaluation = null,
                string note = "")
            {
                if (actionIntent == null) throw new ArgumentNullException(nameof(actionIntent));

                var snapshot = new IntentAuditSnapshot(
                    Guid.NewGuid(),
                    recordedSimSeconds,
                    actionIntent,
                    planning,
                    gateEvaluation,
                    note);

                _history.Add(snapshot);
                return snapshot;
            }

            public IntentAuditReadOnly SnapshotReadOnly()
            {
                return new IntentAuditReadOnly(_history);
            }
        }
    }
}
