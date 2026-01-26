namespace FarmSim.Application.Simulation.Tick.Model
{
    /// <summary>
    /// Outcome of a single simulation step execution during a tick.
    /// Kept intentionally small and stable for diagnostics and auditing.
    /// </summary>
    public enum SimulationStepExecutionStatus
    {
        Success = 0,
        Skipped = 1,
        Denied = 2,
        Failed = 3
    }
}
