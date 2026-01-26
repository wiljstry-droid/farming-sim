using System;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Factory helpers for building immutable execution plan objects from already-resolved inputs.
    /// No lookup/traversal. No validation. No gate evaluation. No mutation.
    /// </summary>
    public static class FieldPatchActionExecutionPlanFactoryModel
    {
        /// <summary>
        /// Creates an immutable execution plan bound to a single intent and planning context,
        /// with frozen scope + frozen preconditions snapshot.
        /// Caller must provide all resolved inputs explicitly.
        /// </summary>
        public static FieldPatchActionExecutionPlanModel.ExecutionPlan CreatePlan(
            FieldPatchActionExecutionPlanModel.ExecutionPlanId planId,
            Guid correlationId,
            Guid? sourceAuditId,
            double createdSimSeconds,
            object actionIntent,
            FieldPatchActionPlanningContextModel.PlanningContext planningContext,
            FieldPatchActionExecutionPlanModel.PlanScope scope,
            FieldPatchActionExecutionPlanModel.ExecutionPreconditionsSnapshot preconditionsSnapshot,
            string note)
        {
            return new FieldPatchActionExecutionPlanModel.ExecutionPlan(
                planId,
                correlationId,
                sourceAuditId,
                createdSimSeconds,
                actionIntent,
                planningContext,
                scope,
                preconditionsSnapshot,
                note
            );
        }

        public static FieldPatchActionExecutionPlanModel.PlanScope CreateScopeExplicit(
            FieldPatchCoord[] resolvedTargets)
        {
            return new FieldPatchActionExecutionPlanModel.PlanScope(
                FieldPatchActionExecutionPlanModel.PlanScopeSource.Explicit,
                derivedFromVessel: null,
                derivedFromShapeId: null,
                targetPatches: FieldPatchActionExecutionPlanModel.Freeze.Patches(resolvedTargets)
            );
        }

        public static FieldPatchActionExecutionPlanModel.PlanScope CreateScopeFromAssignment(
            FieldVesselId derivedFromVessel,
            FieldPatchCoord[] resolvedTargets)
        {
            return new FieldPatchActionExecutionPlanModel.PlanScope(
                FieldPatchActionExecutionPlanModel.PlanScopeSource.AssignmentDerived,
                derivedFromVessel: derivedFromVessel,
                derivedFromShapeId: null,
                targetPatches: FieldPatchActionExecutionPlanModel.Freeze.Patches(resolvedTargets)
            );
        }

        public static FieldPatchActionExecutionPlanModel.PlanScope CreateScopeFromShape(
            Guid derivedFromShapeId,
            FieldPatchCoord[] resolvedTargets)
        {
            return new FieldPatchActionExecutionPlanModel.PlanScope(
                FieldPatchActionExecutionPlanModel.PlanScopeSource.ShapeDerived,
                derivedFromVessel: null,
                derivedFromShapeId: derivedFromShapeId,
                targetPatches: FieldPatchActionExecutionPlanModel.Freeze.Patches(resolvedTargets)
            );
        }

        public static FieldPatchActionExecutionPlanModel.ExecutionPreconditionsSnapshot CreatePreconditionsSnapshot(
            double capturedSimSeconds,
            FieldPatchActionPlanningContextModel.Precondition[] preconditions)
        {
            return new FieldPatchActionExecutionPlanModel.ExecutionPreconditionsSnapshot(
                capturedSimSeconds,
                FieldPatchActionExecutionPlanModel.Freeze.Preconditions(preconditions)
            );
        }
    }
}
