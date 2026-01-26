namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// Read-only execution context provided to simulation steps.
    /// Engine-agnostic: no UnityEngine references allowed.
    /// </summary>
    public interface ISimulationStepContext
    {
        /// <summary>
        /// Current simulation tick index (monotonic).
        /// </summary>
        long Tick { get; }

        /// <summary>
        /// Authoritative simulation delta time for this execution.
        /// </summary>
        double DeltaSeconds { get; }
    }
}
