using System;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 20 (DATA ONLY)
    /// Scope & target resolution inputs (frozen at plan-creation time).
    /// This does NOT perform traversal/lookup. It only records what was resolved and how.
    /// </summary>
    public static class FieldPatchActionExecutionPlanScopeResolutionModel
    {
        /// <summary>
        /// Declares how targets were resolved for the plan.
        /// </summary>
        public enum ResolutionKind
        {
            ExplicitTargets = 0,
            AssignmentDerivedTargets = 1,
            ShapeDerivedTargets = 2
        }

        /// <summary>
        /// Immutable, inspectable record of scope resolution inputs + the final resolved target set.
        /// </summary>
        public sealed class ScopeResolution
        {
            public ResolutionKind Kind { get; }
            public FieldVesselId? DerivedFromVessel { get; }
            public Guid? DerivedFromShapeId { get; }

            /// <summary>
            /// The final resolved target patches (frozen, read-only).
            /// </summary>
            public ReadOnlyCollection<FieldPatchCoord> ResolvedTargets => _resolvedTargets;

            private readonly ReadOnlyCollection<FieldPatchCoord> _resolvedTargets;

            internal ScopeResolution(
                ResolutionKind kind,
                FieldVesselId? derivedFromVessel,
                Guid? derivedFromShapeId,
                ReadOnlyCollection<FieldPatchCoord> resolvedTargets)
            {
                Kind = kind;
                DerivedFromVessel = derivedFromVessel;
                DerivedFromShapeId = derivedFromShapeId;
                _resolvedTargets = resolvedTargets ?? new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());
            }
        }

        /// <summary>
        /// Freeze helpers (internal) to protect immutability and prevent external mutation.
        /// </summary>
        public static class Freeze
        {
            internal static ReadOnlyCollection<FieldPatchCoord> Targets(FieldPatchCoord[] targets)
            {
                if (targets == null || targets.Length == 0)
                    return new ReadOnlyCollection<FieldPatchCoord>(Array.Empty<FieldPatchCoord>());

                var copy = new FieldPatchCoord[targets.Length];
                Array.Copy(targets, copy, targets.Length);
                return new ReadOnlyCollection<FieldPatchCoord>(copy);
            }
        }
    }
}
