namespace FarmSim.Application.Simulation.Tick.Model
{
    /// <summary>
    /// Authoritative outcome kind for a simulation step execution.
    /// Deterministic, inspectable, and stable across runs.
    /// </summary>
    public enum SimulationStepOutcomeKind
    {
        Success = 0,
        Skipped = 1,
        Denied = 2,
        Failed = 3
    }
}
