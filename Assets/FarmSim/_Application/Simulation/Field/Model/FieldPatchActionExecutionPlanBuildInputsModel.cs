using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Bundles all explicit, already-resolved inputs required to build an ExecutionPlan.
    /// This prevents "implicit inference" by forcing the caller to provide scope + snapshots.
    /// </summary>
    public static class FieldPatchActionExecutionPlanBuildInputsModel
    {
        public sealed class BuildInputs
        {
            /// <summary>
            /// Stable plan identity.
            /// </summary>
            public FieldPatchActionExecutionPlanModel.ExecutionPlanId PlanId { get; }

            /// <summary>
            /// Correlation ID that ties intent/audit/gate/plans together.
            /// </summary>
            public Guid CorrelationId { get; }

            /// <summary>
            /// Optional upstream audit record id (Phase 19).
            /// </summary>
            public Guid? SourceAuditId { get; }

            /// <summary>
            /// Plan creation timestamp in simulation seconds.
            /// </summary>
            public double CreatedSimSeconds { get; }

            /// <summary>
            /// Intent object bound to this plan (Phase 18/19 type is outside this model's concern).
            /// </summary>
            public object ActionIntent { get; }

            /// <summary>
            /// Frozen planning context (Phase 19).
            /// </summary>
            public FieldPatchActionPlanningContextModel.PlanningContext PlanningContext { get; }

            /// <summary>
            /// Frozen scope resolution record (how targets were resolved + final targets).
            /// </summary>
            public FieldPatchActionExecutionPlanScopeResolutionModel.ScopeResolution ScopeResolution { get; }

            /// <summary>
            /// Frozen preconditions snapshot captured at plan creation time.
            /// </summary>
            public FieldPatchActionExecutionPlanModel.ExecutionPreconditionsSnapshot PreconditionsSnapshot { get; }

            /// <summary>
            /// Optional plan note for debugging/analytics.
            /// </summary>
            public string Note { get; }

            public BuildInputs(
                FieldPatchActionExecutionPlanModel.ExecutionPlanId planId,
                Guid correlationId,
                Guid? sourceAuditId,
                double createdSimSeconds,
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planningContext,
                FieldPatchActionExecutionPlanScopeResolutionModel.ScopeResolution scopeResolution,
                FieldPatchActionExecutionPlanModel.ExecutionPreconditionsSnapshot preconditionsSnapshot,
                string note)
            {
                PlanId = planId;
                CorrelationId = correlationId;
                SourceAuditId = sourceAuditId;
                CreatedSimSeconds = createdSimSeconds;
                ActionIntent = actionIntent;
                PlanningContext = planningContext;
                ScopeResolution = scopeResolution;
                PreconditionsSnapshot = preconditionsSnapshot;
                Note = note ?? "";
            }
        }
    }
}
