namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// Optional opt-in contract for deterministic step gating.
    ///
    /// IMPORTANT:
    /// - Gate evaluation MUST be called only from SimulationTickAuthority during ordered step execution.
    /// - Implementations must be deterministic: same inputs => same decision (no Unity timing, no randomness).
    /// - If denied, the step's Execute/ExecuteWithOutcome must NOT run for that tick.
    /// </summary>
    public interface ISimulationStepGate
    {
        /// <summary>
        /// Evaluate whether this step is allowed to execute for the current tick.
        /// Return false to deny execution.
        ///
        /// reasonCode: stable, machine-readable code (non-empty).
        /// reasonDetail: optional human detail (may be null/empty).
        /// </summary>
        bool IsAllowed(out string reasonCode, out string reasonDetail);
    }
}
