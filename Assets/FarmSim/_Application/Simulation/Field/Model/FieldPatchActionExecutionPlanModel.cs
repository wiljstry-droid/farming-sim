using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Deterministic, inspectable execution plans derived from action intents + planning context.
    /// No execution. No scheduling. No persistence. No MonoBehaviours.
    /// </summary>
    public static class FieldPatchActionExecutionPlanModel
    {
        /// <summary>
        /// Stable identity for an execution plan.
        /// </summary>
        public readonly struct ExecutionPlanId : IEquatable<ExecutionPlanId>
        {
            public readonly Guid Value;

            public ExecutionPlanId(Guid value)
            {
                Value = value;
            }

            public bool Equals(ExecutionPlanId other) => Value.Equals(other.Value);
            public override bool Equals(object obj) => obj is ExecutionPlanId other && Equals(other);
            public override int GetHashCode() => Value.GetHashCode();

            public static bool operator ==(ExecutionPlanId a, ExecutionPlanId b) => a.Equals(b);
            public static bool operator !=(ExecutionPlanId a, ExecutionPlanId b) => !a.Equals(b);

            public override string ToString() => $"ExecutionPlanId({Value})";
        }

        /// <summary>
        /// Declares how plan scope was resolved (data-only; no lookup/traversal here).
        /// </summary>
        public enum PlanScopeSource
        {
            Explicit = 0,
            AssignmentDerived = 1,
            ShapeDerived = 2
        }

        /// <summary>
        /// Frozen set of targets a plan applies to.
        /// This is "already resolved" at plan creation time.
        /// </summary>
        public sealed class PlanScope
        {
            public PlanScopeSource Source { get; }
            public FieldVesselId? DerivedFromVessel { get; }
            public Guid? DerivedFromShapeId { get; }

            /// <summary>
            /// Fully resolved patch targets (read-only).
            /// </summary>
            public ReadOnlyCollection<FieldPatchCoord> TargetPatches => _targetPatches;

            private readonly ReadOnlyCollection<FieldPatchCoord> _targetPatches;

            internal PlanScope(
                PlanScopeSource source,
                FieldVesselId? derivedFromVessel,
                Guid? derivedFromShapeId,
                ReadOnlyCollection<FieldPatchCoord> targetPatches)
            {
                Source = source;
                DerivedFromVessel = derivedFromVessel;
                DerivedFromShapeId = derivedFromShapeId;
                _targetPatches = targetPatches ?? new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());
            }
        }

        /// <summary>
        /// Snapshot of preconditions captured at plan creation time for later validation/replay.
        /// No evaluation logic here.
        /// </summary>
        public sealed class ExecutionPreconditionsSnapshot
        {
            public double CapturedSimSeconds { get; }

            /// <summary>
            /// Preconditions copied from planning context and frozen here.
            /// </summary>
            public ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition> Preconditions => _preconditions;

            private readonly ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition> _preconditions;

            internal ExecutionPreconditionsSnapshot(
                double capturedSimSeconds,
                ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition> preconditions)
            {
                CapturedSimSeconds = capturedSimSeconds;
                _preconditions = preconditions ?? new ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition>(
                    Array.Empty<FieldPatchActionPlanningContextModel.Precondition>());
            }
        }

        /// <summary>
        /// Immutable execution plan bound to exactly one action intent and planning context.
        /// This does not execute. It only describes what execution SHOULD do later.
        /// </summary>
        public sealed class ExecutionPlan
        {
            public ExecutionPlanId PlanId { get; }
            public Guid CorrelationId { get; }
            public Guid? SourceAuditId { get; }

            public double CreatedSimSeconds { get; }

            /// <summary>
            /// The original action intent (PHASE 18). Stored as object to avoid coupling.
            /// </summary>
            public object ActionIntent { get; }

            /// <summary>
            /// Planning context bound at creation time (PHASE 19).
            /// </summary>
            public FieldPatchActionPlanningContextModel.PlanningContext PlanningContext { get; }

            /// <summary>
            /// Frozen scope/targets for this plan (already resolved).
            /// </summary>
            public PlanScope Scope { get; }

            /// <summary>
            /// Frozen preconditions snapshot for later validation/replay.
            /// </summary>
            public ExecutionPreconditionsSnapshot PreconditionsSnapshot { get; }

            public string Note { get; }

            internal ExecutionPlan(
                ExecutionPlanId planId,
                Guid correlationId,
                Guid? sourceAuditId,
                double createdSimSeconds,
                object actionIntent,
                FieldPatchActionPlanningContextModel.PlanningContext planningContext,
                PlanScope scope,
                ExecutionPreconditionsSnapshot preconditionsSnapshot,
                string note)
            {
                PlanId = planId;
                CorrelationId = correlationId;
                SourceAuditId = sourceAuditId;
                CreatedSimSeconds = createdSimSeconds;

                ActionIntent = actionIntent;
                PlanningContext = planningContext;
                Scope = scope;
                PreconditionsSnapshot = preconditionsSnapshot;
                Note = note ?? "";
            }
        }

        /// <summary>
        /// Data-only helper container for creating frozen collections without exposing mutation.
        /// (Internal only; no inference/lookup.)
        /// </summary>
        public static class Freeze
        {
            internal static ReadOnlyCollection<FieldPatchCoord> Patches(FieldPatchCoord[] patches)
            {
                if (patches == null || patches.Length == 0)
                    return new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());

                var copy = new FieldPatchCoord[patches.Length];
                Array.Copy(patches, copy, patches.Length);
                return new ReadOnlyCollection<FieldPatchCoord>(copy);
            }

            internal static ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition> Preconditions(
                FieldPatchActionPlanningContextModel.Precondition[] preconditions)
            {
                if (preconditions == null || preconditions.Length == 0)
                    return new ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition>(
                        Array.Empty<FieldPatchActionPlanningContextModel.Precondition>());

                var copy = new FieldPatchActionPlanningContextModel.Precondition[preconditions.Length];
                Array.Copy(preconditions, copy, preconditions.Length);
                return new ReadOnlyCollection<FieldPatchActionPlanningContextModel.Precondition>(copy);
            }
        }
    }
}
