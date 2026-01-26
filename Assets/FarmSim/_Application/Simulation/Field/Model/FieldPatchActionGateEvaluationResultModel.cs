using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// PHASE 19 (DATA ONLY)
    /// Canonical, immutable outcome of evaluating an action intent at the Action Gate.
    /// No execution. No state mutation. No scheduling. No persistence.
    /// </summary>
    public static class FieldPatchActionGateEvaluationResultModel
    {
        public enum Decision
        {
            Allow = 0,
            Deny = 1,
            Indeterminate = 2
        }

        /// <summary>
        /// Structured decision code (machine-friendly) optionally paired with a short human note.
        /// This is intentionally generic; concrete code catalogs can be added later without refactors.
        /// </summary>
        public readonly struct DecisionCode : IEquatable<DecisionCode>
        {
            public readonly string Value;
            public readonly string Note;

            public DecisionCode(string value, string note = "")
            {
                Value = value ?? "";
                Note = note ?? "";
            }

            public bool Equals(DecisionCode other)
                => string.Equals(Value, other.Value, StringComparison.Ordinal)
                   && string.Equals(Note, other.Note, StringComparison.Ordinal);

            public override bool Equals(object obj)
                => obj is DecisionCode other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 31) + (Value != null ? Value.GetHashCode() : 0);
                    hash = (hash * 31) + (Note != null ? Note.GetHashCode() : 0);
                    return hash;
                }
            }

            public static bool operator ==(DecisionCode a, DecisionCode b) => a.Equals(b);
            public static bool operator !=(DecisionCode a, DecisionCode b) => !a.Equals(b);

            public override string ToString()
                => string.IsNullOrEmpty(Note) ? Value : $"{Value} ({Note})";
        }

        [Flags]
        public enum ReasonFlags
        {
            None = 0,

            // High-level categories (intentionally broad; do not imply any execution)
            ShapeInvalid = 1 << 0,
            PreconditionsMissing = 1 << 1,
            TimeWindowConflict = 1 << 2,
            RiskTooHigh = 1 << 3,
            ConfidenceTooLow = 1 << 4,
            Unknown = 1 << 30
        }

        /// <summary>
        /// Immutable evaluation result object.
        /// </summary>
        public sealed class EvaluationResult
        {
            public Decision GateDecision { get; }
            public ReasonFlags Flags { get; }

            /// <summary>
            /// Machine-friendly structured codes explaining the decision.
            /// </summary>
            public ReadOnlyCollection<DecisionCode> Codes { get; }

            private EvaluationResult(
                Decision decision,
                ReasonFlags flags,
                ReadOnlyCollection<DecisionCode> codes)
            {
                GateDecision = decision;
                Flags = flags;
                Codes = codes ?? new ReadOnlyCollection<DecisionCode>(Array.Empty<DecisionCode>());
            }

            public static EvaluationResult Allow(params DecisionCode[] codes)
                => new EvaluationResult(Decision.Allow, ReasonFlags.None, WrapCodes(codes));

            public static EvaluationResult Deny(ReasonFlags flags, params DecisionCode[] codes)
                => new EvaluationResult(Decision.Deny, flags == ReasonFlags.None ? ReasonFlags.Unknown : flags, WrapCodes(codes));

            public static EvaluationResult Indeterminate(ReasonFlags flags, params DecisionCode[] codes)
                => new EvaluationResult(Decision.Indeterminate, flags == ReasonFlags.None ? ReasonFlags.Unknown : flags, WrapCodes(codes));

            private static ReadOnlyCollection<DecisionCode> WrapCodes(DecisionCode[] codes)
            {
                if (codes == null || codes.Length == 0)
                    return new ReadOnlyCollection<DecisionCode>(Array.Empty<DecisionCode>());

                // Copy to prevent external mutation of the provided array.
                var copy = new DecisionCode[codes.Length];
                Array.Copy(codes, copy, codes.Length);
                return new ReadOnlyCollection<DecisionCode>(copy);
            }
        }
    }
}
