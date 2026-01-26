using System;

namespace FarmSim.Application.Simulation.Tick.Model
{
    /// <summary>
    /// Immutable, deterministic outcome contract for a single step execution.
    ///
    /// Goals:
    /// - Explicit (no inference from logs/exceptions)
    /// - Deterministic / stable across runs
    /// - Read-only once produced
    /// - Structured reason/error information (foundation-level)
    /// </summary>
    public sealed class SimulationStepOutcome
    {
        public SimulationStepOutcomeKind Kind { get; }

        /// <summary>
        /// For Skipped/Denied: a stable, machine-friendly reason code.
        /// Example: "GatePaused", "NoDelta", "MissingAuthority", "NotReady".
        /// Null for Success, typically null for Failed (use error fields).
        /// </summary>
        public string ReasonCode { get; }

        /// <summary>
        /// Optional human-readable detail. Should remain deterministic when possible.
        /// </summary>
        public string ReasonDetail { get; }

        /// <summary>
        /// For Failed: the exception type name (stable).
        /// Null otherwise.
        /// </summary>
        public string ErrorType { get; }

        /// <summary>
        /// For Failed: the exception message (may vary by environment, but is inspectable).
        /// Null otherwise.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Deterministic hash derived from (ErrorType + ErrorMessage) using a stable algorithm.
        /// 0 when not Failed.
        /// </summary>
        public int ErrorStableHash { get; }

        private SimulationStepOutcome(
            SimulationStepOutcomeKind kind,
            string reasonCode,
            string reasonDetail,
            string errorType,
            string errorMessage,
            int errorStableHash)
        {
            Kind = kind;
            ReasonCode = reasonCode;
            ReasonDetail = reasonDetail;
            ErrorType = errorType;
            ErrorMessage = errorMessage;
            ErrorStableHash = errorStableHash;
        }

        public static SimulationStepOutcome Success()
            => new SimulationStepOutcome(SimulationStepOutcomeKind.Success, null, null, null, null, 0);

        public static SimulationStepOutcome Skipped(string reasonCode, string reasonDetail = null)
            => new SimulationStepOutcome(SimulationStepOutcomeKind.Skipped, Safe(reasonCode), Safe(reasonDetail), null, null, 0);

        public static SimulationStepOutcome Denied(string reasonCode, string reasonDetail = null)
            => new SimulationStepOutcome(SimulationStepOutcomeKind.Denied, Safe(reasonCode), Safe(reasonDetail), null, null, 0);

        public static SimulationStepOutcome Failed(Exception exception)
        {
            if (exception == null)
                return new SimulationStepOutcome(SimulationStepOutcomeKind.Failed, null, null, "UnknownException", null, StableHash("UnknownException", null));

            var type = exception.GetType().FullName ?? exception.GetType().Name ?? "Exception";
            var msg = exception.Message;

            return new SimulationStepOutcome(
                SimulationStepOutcomeKind.Failed,
                null,
                null,
                type,
                msg,
                StableHash(type, msg));
        }

        private static string Safe(string s)
        {
            // Enforce deterministic empties (no null-vs-empty drift in consumers).
            return string.IsNullOrEmpty(s) ? string.Empty : s;
        }

        private static int StableHash(string a, string b)
        {
            // FNV-1a 32-bit over UTF-16 chars (deterministic in .NET).
            unchecked
            {
                const int fnvOffset = unchecked((int)2166136261);
                const int fnvPrime = 16777619;

                int h = fnvOffset;

                if (!string.IsNullOrEmpty(a))
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        h ^= a[i];
                        h *= fnvPrime;
                    }
                }

                h ^= '|';
                h *= fnvPrime;

                if (!string.IsNullOrEmpty(b))
                {
                    for (int i = 0; i < b.Length; i++)
                    {
                        h ^= b[i];
                        h *= fnvPrime;
                    }
                }

                return h;
            }
        }

        public override string ToString()
        {
            // Useful for debug logs (still deterministic for Kind/ReasonCode).
            switch (Kind)
            {
                case SimulationStepOutcomeKind.Success:
                    return "Success";

                case SimulationStepOutcomeKind.Skipped:
                    return string.IsNullOrEmpty(ReasonCode) ? "Skipped" : $"Skipped({ReasonCode})";

                case SimulationStepOutcomeKind.Denied:
                    return string.IsNullOrEmpty(ReasonCode) ? "Denied" : $"Denied({ReasonCode})";

                case SimulationStepOutcomeKind.Failed:
                    return $"Failed({ErrorStableHash})";

                default:
                    return Kind.ToString();
            }
        }
    }
}
