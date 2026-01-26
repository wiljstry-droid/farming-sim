using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 19 (DATA ONLY)
    /// Planning context metadata for an action intent.
    /// No schedulers, queues, execution, persistence, or MonoBehaviours.
    /// </summary>
    public static class FieldPatchActionPlanningContextModel
    {
        /// <summary>
        /// Optional temporal window for when an action would be valid to execute.
        /// Uses simulation time concepts (no Unity time types).
        /// </summary>
        public readonly struct TemporalWindow : IEquatable<TemporalWindow>
        {
            /// <summary>
            /// Earliest allowable simulation timestamp (inclusive). Null means "no earliest bound".
            /// </summary>
            public readonly double? EarliestSimSeconds;

            /// <summary>
            /// Latest allowable simulation timestamp (inclusive). Null means "no latest bound".
            /// </summary>
            public readonly double? LatestSimSeconds;

            public TemporalWindow(double? earliestSimSeconds, double? latestSimSeconds)
            {
                EarliestSimSeconds = earliestSimSeconds;
                LatestSimSeconds = latestSimSeconds;
            }

            public bool HasAnyBound => EarliestSimSeconds.HasValue || LatestSimSeconds.HasValue;

            public bool Equals(TemporalWindow other)
                => EarliestSimSeconds.Equals(other.EarliestSimSeconds)
                   && LatestSimSeconds.Equals(other.LatestSimSeconds);

            public override bool Equals(object obj)
                => obj is TemporalWindow other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 31) + (EarliestSimSeconds.HasValue ? EarliestSimSeconds.Value.GetHashCode() : 0);
                    hash = (hash * 31) + (LatestSimSeconds.HasValue ? LatestSimSeconds.Value.GetHashCode() : 0);
                    return hash;
                }
            }

            public static bool operator ==(TemporalWindow a, TemporalWindow b) => a.Equals(b);
            public static bool operator !=(TemporalWindow a, TemporalWindow b) => !a.Equals(b);

            public override string ToString()
            {
                string e = EarliestSimSeconds.HasValue ? EarliestSimSeconds.Value.ToString("0.###") : "-inf";
                string l = LatestSimSeconds.HasValue ? LatestSimSeconds.Value.ToString("0.###") : "+inf";
                return $"[{e} .. {l}]";
            }
        }

        /// <summary>
        /// Optional precondition descriptor (data only). This does not evaluate anything.
        /// </summary>
        public readonly struct Precondition : IEquatable<Precondition>
        {
            /// <summary>
            /// Machine-friendly identifier, e.g. "SoilMoistureKnown" or "HasAssignment".
            /// </summary>
            public readonly string Id;

            /// <summary>
            /// Optional human note (short).
            /// </summary>
            public readonly string Note;

            public Precondition(string id, string note = "")
            {
                Id = id ?? "";
                Note = note ?? "";
            }

            public bool Equals(Precondition other)
                => string.Equals(Id, other.Id, StringComparison.Ordinal)
                   && string.Equals(Note, other.Note, StringComparison.Ordinal);

            public override bool Equals(object obj)
                => obj is Precondition other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 31) + (Id != null ? Id.GetHashCode() : 0);
                    hash = (hash * 31) + (Note != null ? Note.GetHashCode() : 0);
                    return hash;
                }
            }

            public static bool operator ==(Precondition a, Precondition b) => a.Equals(b);
            public static bool operator !=(Precondition a, Precondition b) => !a.Equals(b);

            public override string ToString()
                => string.IsNullOrEmpty(Note) ? Id : $"{Id} ({Note})";
        }

        /// <summary>
        /// Risk/confidence metadata (data only).
        /// Scores are normalized to [0..1] by convention; no enforcement or evaluation here.
        /// </summary>
        public readonly struct RiskConfidence : IEquatable<RiskConfidence>
        {
            public readonly double RiskScore01;
            public readonly double Confidence01;

            public RiskConfidence(double riskScore01, double confidence01)
            {
                RiskScore01 = riskScore01;
                Confidence01 = confidence01;
            }

            public bool Equals(RiskConfidence other)
                => RiskScore01.Equals(other.RiskScore01) && Confidence01.Equals(other.Confidence01);

            public override bool Equals(object obj)
                => obj is RiskConfidence other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 31) + RiskScore01.GetHashCode();
                    hash = (hash * 31) + Confidence01.GetHashCode();
                    return hash;
                }
            }

            public static bool operator ==(RiskConfidence a, RiskConfidence b) => a.Equals(b);
            public static bool operator !=(RiskConfidence a, RiskConfidence b) => !a.Equals(b);

            public override string ToString()
                => $"risk={RiskScore01:0.###} conf={Confidence01:0.###}";
        }

        /// <summary>
        /// Immutable planning context object to accompany an action intent.
        /// </summary>
        public sealed class PlanningContext
        {
            public TemporalWindow Window { get; }
            public RiskConfidence Meta { get; }

            public ReadOnlyCollection<Precondition> Preconditions { get; }

            /// <summary>
            /// Optional short note for debugging / analytics (no gameplay UI).
            /// </summary>
            public string Note { get; }

            private PlanningContext(
                TemporalWindow window,
                RiskConfidence meta,
                ReadOnlyCollection<Precondition> preconditions,
                string note)
            {
                Window = window;
                Meta = meta;
                Preconditions = preconditions ?? new ReadOnlyCollection<Precondition>(Array.Empty<Precondition>());
                Note = note ?? "";
            }

            public static PlanningContext Create(
                TemporalWindow window,
                RiskConfidence meta,
                Precondition[] preconditions = null,
                string note = "")
            {
                return new PlanningContext(
                    window,
                    meta,
                    WrapPreconditions(preconditions),
                    note);
            }

            private static ReadOnlyCollection<Precondition> WrapPreconditions(Precondition[] preconditions)
            {
                if (preconditions == null || preconditions.Length == 0)
                    return new ReadOnlyCollection<Precondition>(Array.Empty<Precondition>());

                var copy = new Precondition[preconditions.Length];
                Array.Copy(preconditions, copy, preconditions.Length);
                return new ReadOnlyCollection<Precondition>(copy);
            }
        }
    }
}
