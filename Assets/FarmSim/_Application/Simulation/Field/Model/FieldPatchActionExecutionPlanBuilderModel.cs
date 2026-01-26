using System;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Builds immutable ExecutionPlan objects from explicit BuildInputs.
    /// No lookup, no validation, no gate evaluation, no mutation.
    /// </summary>
    public static class FieldPatchActionExecutionPlanBuilderModel
    {
        public static FieldPatchActionExecutionPlanModel.ExecutionPlan Build(
            FieldPatchActionExecutionPlanBuildInputsModel.BuildInputs inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));

            // ScopeResolution is recorded separately, but ExecutionPlan stores the frozen PlanScope.
            // Convert directly without inference.
            var scope = ConvertScopeResolutionToPlanScope(inputs.ScopeResolution);

            return FieldPatchActionExecutionPlanFactoryModel.CreatePlan(
                inputs.PlanId,
                inputs.CorrelationId,
                inputs.SourceAuditId,
                inputs.CreatedSimSeconds,
                inputs.ActionIntent,
                inputs.PlanningContext,
                scope,
                inputs.PreconditionsSnapshot,
                inputs.Note
            );
        }

        private static FieldPatchActionExecutionPlanModel.PlanScope ConvertScopeResolutionToPlanScope(
            FieldPatchActionExecutionPlanScopeResolutionModel.ScopeResolution resolution)
        {
            if (resolution == null) throw new ArgumentNullException(nameof(resolution));

            // Build PlanScope directly from resolution record; targets already resolved.
            switch (resolution.Kind)
            {
                case FieldPatchActionExecutionPlanScopeResolutionModel.ResolutionKind.ExplicitTargets:
                {
                    return FieldPatchActionExecutionPlanFactoryModel.CreateScopeExplicit(
                        resolvedTargets: ToArray(resolution.ResolvedTargets)
                    );
                }

                case FieldPatchActionExecutionPlanScopeResolutionModel.ResolutionKind.AssignmentDerivedTargets:
                {
                    if (!resolution.DerivedFromVessel.HasValue)
                        throw new InvalidOperationException("ScopeResolution missing DerivedFromVessel.");

                    return FieldPatchActionExecutionPlanFactoryModel.CreateScopeFromAssignment(
                        derivedFromVessel: resolution.DerivedFromVessel.Value,
                        resolvedTargets: ToArray(resolution.ResolvedTargets)
                    );
                }

                case FieldPatchActionExecutionPlanScopeResolutionModel.ResolutionKind.ShapeDerivedTargets:
                {
                    if (!resolution.DerivedFromShapeId.HasValue)
                        throw new InvalidOperationException("ScopeResolution missing DerivedFromShapeId.");

                    return FieldPatchActionExecutionPlanFactoryModel.CreateScopeFromShape(
                        derivedFromShapeId: resolution.DerivedFromShapeId.Value,
                        resolvedTargets: ToArray(resolution.ResolvedTargets)
                    );
                }

                default:
                    throw new InvalidOperationException($"Unknown ResolutionKind: {resolution.Kind}");
            }
        }

        private static FieldPatchCoord[] ToArray(
            System.Collections.ObjectModel.ReadOnlyCollection<FieldPatchCoord> targets)
        {
            if (targets == null || targets.Count == 0)
                return Array.Empty<FieldPatchCoord>();

            var arr = new FieldPatchCoord[targets.Count];
            for (int i = 0; i < targets.Count; i++)
                arr[i] = targets[i];

            return arr;
        }
    }
}
