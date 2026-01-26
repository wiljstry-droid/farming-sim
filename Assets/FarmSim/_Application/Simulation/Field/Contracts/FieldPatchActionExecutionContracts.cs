using System;
using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field.Contracts
{
    /// <summary>
    /// PHASE 19 (CONTRACTS ONLY)
    /// Execution contract interfaces (no implementations).
    /// These define how execution will eventually be plugged in, without scheduling or mutation here.
    /// </summary>
    public static class FieldPatchActionExecutionContracts
    {
        /// <summary>
        /// Read-only execution context available to action executors.
        /// Must not expose mutable FieldPatch state in this phase.
        /// </summary>
        public interface IActionExecutionContext
        {
            /// <summary>
            /// Current simulation time in seconds.
            /// </summary>
            double SimSeconds { get; }
        }

        /// <summary>
        /// Represents a prepared execution plan for an action intent (data-only handle).
        /// No queues, no coroutines, no async requirements implied.
        /// </summary>
        public interface IActionExecutionPlan
        {
            /// <summary>
            /// Unique id for correlating plans and outcomes.
            /// </summary>
            Guid PlanId { get; }

            /// <summary>
            /// Original intent object (PHASE 18).
            /// </summary>
            object ActionIntent { get; }

            /// <summary>
            /// Planning context metadata (PHASE 19).
            /// </summary>
            FieldPatchActionPlanningContextModel.PlanningContext Planning { get; }
        }

        /// <summary>
        /// Result of executing an action plan (data-only outcome contract).
        /// No FieldPatch mutation is performed here; mutation will be defined in later phases.
        /// </summary>
        public interface IActionExecutionOutcome
        {
            Guid PlanId { get; }

            /// <summary>
            /// Gate evaluation that applied at execution time (or closest known evaluation).
            /// </summary>
            FieldPatchActionGateEvaluationResultModel.EvaluationResult GateEvaluation { get; }

            /// <summary>
            /// True if the plan was executed to completion as intended (definition evolves later).
            /// </summary>
            bool Completed { get; }

            /// <summary>
            /// Optional structured outcome code.
            /// </summary>
            string OutcomeCode { get; }

            /// <summary>
            /// Optional short note for debugging/analytics.
            /// </summary>
            string Note { get; }
        }

        /// <summary>
        /// Builds an execution plan from an intent + planning metadata.
        /// No scheduling or running implied.
        /// </summary>
        public interface IActionPlanBuilder
        {
            IActionExecutionPlan BuildPlan(
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planning);
        }

        /// <summary>
        /// Evaluates whether an action intent/plan is allowed at the gate.
        /// This is reasoning-only; no execution.
        /// </summary>
        public interface IActionGateEvaluator
        {
            FieldPatchActionGateEvaluationResultModel.EvaluationResult Evaluate(
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planning,
                IActionExecutionContext context);
        }

        /// <summary>
        /// Executes a previously built plan and returns an outcome.
        /// No implementations in PHASE 19.
        /// </summary>
        public interface IActionExecutor
        {
            IActionExecutionOutcome Execute(
                IActionExecutionPlan plan,
                IActionExecutionContext context);
        }
    }
}
