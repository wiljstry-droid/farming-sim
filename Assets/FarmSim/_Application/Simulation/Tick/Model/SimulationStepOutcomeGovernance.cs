using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick.Model.Governance
{
    /// <summary>
    /// Canonical governance ("law") for SimulationStepOutcome.
    ///
    /// This class defines what qualifies as a valid outcome:
    /// - Outcomes must be explicit and non-null.
    /// - Each OutcomeKind has a strict payload contract.
    /// - Reason codes are governed to prevent semantic drift.
    ///
    /// This does not change mechanics. It only provides validation.
    /// Tick/pipeline/diagnostics may choose to enforce it.
    /// </summary>
    public static class SimulationStepOutcomeGovernance
    {
        public static SimulationStepOutcomeGovernanceViolation Validate(SimulationStepOutcome outcome)
        {
            if (outcome == null)
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.OutcomeNull,
                    "Outcome is null.");

            if (!IsDefinedKind(outcome.Kind))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.KindUndefined,
                    $"Outcome.Kind is undefined: {outcome.Kind}");

            switch (outcome.Kind)
            {
                case SimulationStepOutcomeKind.Success:
                    return ValidateSuccess(outcome);

                case SimulationStepOutcomeKind.Skipped:
                    return ValidateSkipped(outcome);

                case SimulationStepOutcomeKind.Denied:
                    return ValidateDenied(outcome);

                case SimulationStepOutcomeKind.Failed:
                    return ValidateFailed(outcome);

                default:
                    return new SimulationStepOutcomeGovernanceViolation(
                        SimulationStepOutcomeGovernanceViolationKind.KindUndefined,
                        $"Unhandled Outcome.Kind: {outcome.Kind}");
            }
        }

        private static SimulationStepOutcomeGovernanceViolation ValidateSuccess(SimulationStepOutcome o)
        {
            if (!IsNullOrEmpty(o.ReasonCode) || !IsNullOrEmpty(o.ReasonDetail))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SuccessHasReason,
                    $"Success must not carry ReasonCode/ReasonDetail. code='{o.ReasonCode}' detail='{o.ReasonDetail}'");

            if (!IsNullOrEmpty(o.ErrorType) || !IsNullOrEmpty(o.ErrorMessage))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SuccessHasError,
                    $"Success must not carry ErrorType/ErrorMessage. type='{o.ErrorType}' msg='{o.ErrorMessage}'");

            if (o.ErrorStableHash != 0)
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SuccessHasStableHash,
                    $"Success must have ErrorStableHash==0. hash={o.ErrorStableHash}");

            return SimulationStepOutcomeGovernanceViolation.None();
        }

        private static SimulationStepOutcomeGovernanceViolation ValidateSkipped(SimulationStepOutcome o)
        {
            if (IsNullOrEmpty(o.ReasonCode))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SkippedMissingReasonCode,
                    "Skipped must carry a non-empty ReasonCode.");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidReasonCode(o.ReasonCode))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ReasonCodeInvalid,
                    $"Invalid ReasonCode: '{o.ReasonCode}'");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidReasonDetail(o.ReasonDetail))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ReasonDetailInvalid,
                    $"Invalid ReasonDetail: '{o.ReasonDetail}'");

            if (!IsNullOrEmpty(o.ErrorType) || !IsNullOrEmpty(o.ErrorMessage))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SkippedHasError,
                    $"Skipped must not carry ErrorType/ErrorMessage. type='{o.ErrorType}' msg='{o.ErrorMessage}'");

            if (o.ErrorStableHash != 0)
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.SkippedHasStableHash,
                    $"Skipped must have ErrorStableHash==0. hash={o.ErrorStableHash}");

            return SimulationStepOutcomeGovernanceViolation.None();
        }

        private static SimulationStepOutcomeGovernanceViolation ValidateDenied(SimulationStepOutcome o)
        {
            if (IsNullOrEmpty(o.ReasonCode))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.DeniedMissingReasonCode,
                    "Denied must carry a non-empty ReasonCode.");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidReasonCode(o.ReasonCode))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ReasonCodeInvalid,
                    $"Invalid ReasonCode: '{o.ReasonCode}'");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidReasonDetail(o.ReasonDetail))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ReasonDetailInvalid,
                    $"Invalid ReasonDetail: '{o.ReasonDetail}'");

            if (!IsNullOrEmpty(o.ErrorType) || !IsNullOrEmpty(o.ErrorMessage))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.DeniedHasError,
                    $"Denied must not carry ErrorType/ErrorMessage. type='{o.ErrorType}' msg='{o.ErrorMessage}'");

            if (o.ErrorStableHash != 0)
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.DeniedHasStableHash,
                    $"Denied must have ErrorStableHash==0. hash={o.ErrorStableHash}");

            return SimulationStepOutcomeGovernanceViolation.None();
        }

        private static SimulationStepOutcomeGovernanceViolation ValidateFailed(SimulationStepOutcome o)
        {
            if (IsNullOrEmpty(o.ErrorType))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.FailedMissingErrorType,
                    "Failed must carry a non-empty ErrorType.");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidErrorType(o.ErrorType))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ErrorTypeInvalid,
                    $"Invalid ErrorType: '{o.ErrorType}'");

            if (!SimulationStepOutcomeReasonCodePolicy.IsValidErrorMessage(o.ErrorMessage))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.ErrorMessageInvalid,
                    $"Invalid ErrorMessage: '{o.ErrorMessage}'");

            if (!IsNullOrEmpty(o.ReasonCode) || !IsNullOrEmpty(o.ReasonDetail))
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.FailedHasReason,
                    $"Failed must not carry ReasonCode/ReasonDetail. code='{o.ReasonCode}' detail='{o.ReasonDetail}'");

            if (o.ErrorStableHash == 0)
                return new SimulationStepOutcomeGovernanceViolation(
                    SimulationStepOutcomeGovernanceViolationKind.FailedMissingStableHash,
                    "Failed must have a non-zero ErrorStableHash.");

            return SimulationStepOutcomeGovernanceViolation.None();
        }

        private static bool IsNullOrEmpty(string s)
            => string.IsNullOrEmpty(s);

        private static bool IsDefinedKind(SimulationStepOutcomeKind kind)
        {
            return kind == SimulationStepOutcomeKind.Success ||
                   kind == SimulationStepOutcomeKind.Skipped ||
                   kind == SimulationStepOutcomeKind.Denied ||
                   kind == SimulationStepOutcomeKind.Failed;
        }
    }
}
