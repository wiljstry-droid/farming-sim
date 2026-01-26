namespace FarmSim.Application.Simulation.Tick.Model.Governance
{
    /// <summary>
    /// Canonical violation kinds for SimulationStepOutcome governance.
    /// This is intentionally generic and stable: diagnostics and replay tooling can key off it.
    /// </summary>
    public enum SimulationStepOutcomeGovernanceViolationKind
    {
        None = 0,

        OutcomeNull,
        KindUndefined,

        SuccessHasReason,
        SuccessHasError,
        SuccessHasStableHash,

        SkippedMissingReasonCode,
        SkippedHasError,
        SkippedHasStableHash,

        DeniedMissingReasonCode,
        DeniedHasError,
        DeniedHasStableHash,

        FailedMissingErrorType,
        FailedHasReason,
        FailedMissingStableHash,

        ReasonCodeInvalid,
        ReasonDetailInvalid,
        ErrorTypeInvalid,
        ErrorMessageInvalid
    }
}
