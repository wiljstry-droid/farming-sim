using System;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Lightweight read-model projection for UI/analytics/replay systems (later phases).
    /// Derived from an ExecutionPlan but kept decoupled and immutable.
    /// </summary>
    public static class FieldPatchActionExecutionPlanReadModel
    {
        public sealed class ReadModel
        {
            public FieldPatchActionExecutionPlanModel.ExecutionPlanId PlanId { get; }
            public Guid CorrelationId { get; }
            public Guid? SourceAuditId { get; }

            public double CreatedSimSeconds { get; }

            public FieldPatchActionExecutionPlanModel.PlanScopeSource ScopeSource { get; }
            public FieldVesselId? DerivedFromVessel { get; }
            public Guid? DerivedFromShapeId { get; }

            public int TargetCount { get; }
            public ReadOnlyCollection<FieldPatchCoord> Targets => _targets;

            public int PreconditionsCount { get; }

            public string Note { get; }

            private readonly ReadOnlyCollection<FieldPatchCoord> _targets;

            internal ReadModel(
                FieldPatchActionExecutionPlanModel.ExecutionPlanId planId,
                Guid correlationId,
                Guid? sourceAuditId,
                double createdSimSeconds,
                FieldPatchActionExecutionPlanModel.PlanScopeSource scopeSource,
                FieldVesselId? derivedFromVessel,
                Guid? derivedFromShapeId,
                ReadOnlyCollection<FieldPatchCoord> targets,
                int preconditionsCount,
                string note)
            {
                PlanId = planId;
                CorrelationId = correlationId;
                SourceAuditId = sourceAuditId;
                CreatedSimSeconds = createdSimSeconds;

                ScopeSource = scopeSource;
                DerivedFromVessel = derivedFromVessel;
                DerivedFromShapeId = derivedFromShapeId;

                _targets = targets ?? new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());
                TargetCount = _targets.Count;

                PreconditionsCount = preconditionsCount;
                Note = note ?? "";
            }
        }

        public static ReadModel FromPlan(FieldPatchActionExecutionPlanModel.ExecutionPlan plan)
        {
            if (plan == null) throw new ArgumentNullException(nameof(plan));

            var targets = plan.Scope?.TargetPatches
                          ?? new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());

            var preCount = plan.PreconditionsSnapshot?.Preconditions?.Count ?? 0;

            return new ReadModel(
                plan.PlanId,
                plan.CorrelationId,
                plan.SourceAuditId,
                plan.CreatedSimSeconds,
                plan.Scope != null ? plan.Scope.Source : FieldPatchActionExecutionPlanModel.PlanScopeSource.Explicit,
                plan.Scope != null ? plan.Scope.DerivedFromVessel : null,
                plan.Scope != null ? plan.Scope.DerivedFromShapeId : null,
                targets,
                preCount,
                plan.Note
            );
        }
    }
}
