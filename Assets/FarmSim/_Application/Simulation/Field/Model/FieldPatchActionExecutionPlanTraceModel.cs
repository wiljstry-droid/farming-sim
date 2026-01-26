using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Traceability helpers for Execution Plans:
    /// - stable correlation across intent audit, gate evaluation, planning context, and plan creation
    /// - analytics / replay / reporting support
    /// No execution. No scheduling. No persistence.
    /// </summary>
    public static class FieldPatchActionExecutionPlanTraceModel
    {
        /// <summary>
        /// A compact trace link describing how a specific execution plan relates to upstream records.
        /// All IDs here are stable identifiers owned by their respective phases/systems.
        /// This is intentionally data-only: no lookups, no traversal.
        /// </summary>
        public readonly struct ExecutionPlanTraceLink : IEquatable<ExecutionPlanTraceLink>
        {
            /// <summary>
            /// Phase 20 execution plan identity.
            /// </summary>
            public readonly FieldPatchActionExecutionPlanModel.ExecutionPlanId PlanId;

            /// <summary>
            /// Cross-system correlation ID (same value should appear in audit/history records).
            /// </summary>
            public readonly Guid CorrelationId;

            /// <summary>
            /// Optional: ID of the upstream intent audit record associated with this plan (Phase 19).
            /// </summary>
            public readonly Guid? IntentAuditId;

            /// <summary>
            /// Optional: ID of the upstream gate evaluation result record associated with this plan (Phase 19).
            /// </summary>
            public readonly Guid? GateEvaluationResultId;

            /// <summary>
            /// Optional: ID of the intent batch record if this plan was created from a batch (Phase 18/19).
            /// </summary>
            public readonly Guid? IntentBatchId;

            /// <summary>
            /// Plan creation timestamp in simulation seconds.
            /// </summary>
            public readonly double CreatedSimSeconds;

            public ExecutionPlanTraceLink(
                FieldPatchActionExecutionPlanModel.ExecutionPlanId planId,
                Guid correlationId,
                Guid? intentAuditId,
                Guid? gateEvaluationResultId,
                Guid? intentBatchId,
                double createdSimSeconds)
            {
                PlanId = planId;
                CorrelationId = correlationId;
                IntentAuditId = intentAuditId;
                GateEvaluationResultId = gateEvaluationResultId;
                IntentBatchId = intentBatchId;
                CreatedSimSeconds = createdSimSeconds;
            }

            public bool Equals(ExecutionPlanTraceLink other)
            {
                return PlanId == other.PlanId
                       && CorrelationId == other.CorrelationId
                       && IntentAuditId == other.IntentAuditId
                       && GateEvaluationResultId == other.GateEvaluationResultId
                       && IntentBatchId == other.IntentBatchId
                       && CreatedSimSeconds.Equals(other.CreatedSimSeconds);
            }

            public override bool Equals(object obj) => obj is ExecutionPlanTraceLink other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    var hash = 17;
                    hash = (hash * 31) ^ PlanId.GetHashCode();
                    hash = (hash * 31) ^ CorrelationId.GetHashCode();
                    hash = (hash * 31) ^ (IntentAuditId.HasValue ? IntentAuditId.Value.GetHashCode() : 0);
                    hash = (hash * 31) ^ (GateEvaluationResultId.HasValue ? GateEvaluationResultId.Value.GetHashCode() : 0);
                    hash = (hash * 31) ^ (IntentBatchId.HasValue ? IntentBatchId.Value.GetHashCode() : 0);
                    hash = (hash * 31) ^ CreatedSimSeconds.GetHashCode();
                    return hash;
                }
            }

            public static bool operator ==(ExecutionPlanTraceLink a, ExecutionPlanTraceLink b) => a.Equals(b);
            public static bool operator !=(ExecutionPlanTraceLink a, ExecutionPlanTraceLink b) => !a.Equals(b);

            public override string ToString()
            {
                return $"ExecutionPlanTraceLink(plan={PlanId}, corr={CorrelationId}, audit={IntentAuditId}, gate={GateEvaluationResultId}, batch={IntentBatchId}, t={CreatedSimSeconds:0.###})";
            }
        }
    }
}
