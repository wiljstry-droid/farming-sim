using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field.Contracts
{
    /// <summary>
    /// Contract: transforms a single validated intent + frozen planning inputs into
    /// a deterministic, inspectable execution plan (DATA ONLY).
    ///
    /// HARD RULES:
    /// - No execution
    /// - No scheduling
    /// - No traversal / lookup
    /// - No re-evaluating gates
    /// - No mutation
    /// </summary>
    public interface IFieldPatchActionExecutionPlanner
    {
        FieldPatchActionExecutionPlanModel.ExecutionPlan Plan(
            in FieldPatchActionIntent intent,
            in FieldPatchActionPlanningContextModel.PlanningContext planningContext,
            in FieldPatchActionGateEvaluationResultModel.EvaluationResult gateEvaluation,
            in FieldPatchActionIntentAuditModel.IntentAuditSnapshot auditSnapshot
        );
    }
}
