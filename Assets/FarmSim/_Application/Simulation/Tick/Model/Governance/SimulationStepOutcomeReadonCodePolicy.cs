using System;

namespace FarmSim.Application.Simulation.Tick.Model.Governance
{
    /// <summary>
    /// Minimal, non-breaking governance for reason codes and related tokens.
    ///
    /// Goals:
    /// - Prevent silent drift into ad-hoc strings.
    /// - Keep codes comparable, loggable, and safe for replay diagnostics.
    /// - Avoid breaking existing codes by keeping rules conservative.
    ///
    /// Canonical guidance (recommended):
    /// - Use dotted namespaces: STEP.GATE.NoOwner, TICK.Blocked, DOMAIN.Soil.Moisture.OutOfRange, etc.
    /// - Prefer stable codes; put variable/user-facing text in ReasonDetail (and keep it deterministic when possible).
    ///
    /// Reserved roots (guidance, not enforced here):
    /// - SYS.*   : framework-wide system conditions
    /// - TICK.*  : tick-level gating or tick system states
    /// - STEP.*  : step-level gating and scheduling reasons
    /// - DOMAIN.*: domain systems (soil, crops, weather, etc.)
    /// </summary>
    public static class SimulationStepOutcomeReasonCodePolicy
    {
        // Hard limits to keep logs/diagnostics safe.
        public const int MaxCodeLength = 96;
        public const int MaxDetailLength = 256;
        public const int MaxErrorTypeLength = 96;
        public const int MaxErrorMessageLength = 256;

        public static bool IsValidReasonCode(string code)
        {
            return IsValidToken(code, MaxCodeLength, requireDottedNamespace: true);
        }

        public static bool IsValidReasonDetail(string detail)
        {
            // Detail is optional; when present, it must be safe.
            if (string.IsNullOrEmpty(detail))
                return true;

            return IsValidToken(detail, MaxDetailLength, requireDottedNamespace: false, allowColon: true);
        }

        public static bool IsValidErrorType(string errorType)
        {
            // ErrorType should be stable and comparable.
            return IsValidToken(errorType, MaxErrorTypeLength, requireDottedNamespace: false);
        }

        public static bool IsValidErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return true;

            // Message is allowed to be more free-form, but still must be safe and bounded.
            if (message.Length > MaxErrorMessageLength)
                return false;

            if (!string.Equals(message.Trim(), message, StringComparison.Ordinal))
                return false;

            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                if (char.IsControl(c) || c == '\n' || c == '\r' || c == '\t')
                    return false;
            }

            return true;
        }

        private static bool IsValidToken(
            string value,
            int maxLen,
            bool requireDottedNamespace,
            bool allowColon = true)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (value.Length > maxLen)
                return false;

            // Must be trimmed; we never want semantic drift via whitespace.
            if (!string.Equals(value.Trim(), value, StringComparison.Ordinal))
                return false;

            // No whitespace or control chars; no newlines.
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                if (char.IsWhiteSpace(c))
                    return false;

                if (char.IsControl(c) || c == '\n' || c == '\r' || c == '\t')
                    return false;

                // Allowed: A–Z a–z 0–9 . _ - :
                bool ok =
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= '0' && c <= '9') ||
                    (c == '.') ||
                    (c == '_') ||
                    (c == '-') ||
                    (allowColon && c == ':');

                if (!ok)
                    return false;
            }

            // If we require a dotted namespace, force at least one '.' somewhere.
            if (requireDottedNamespace && value.IndexOf('.') < 0)
                return false;

            // Avoid weird edge cases: ".X" or "X." or ".."
            if (value.StartsWith(".", StringComparison.Ordinal) || value.EndsWith(".", StringComparison.Ordinal))
                return false;

            if (value.Contains(".."))
                return false;

            return true;
        }
    }
}
