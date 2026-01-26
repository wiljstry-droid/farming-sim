namespace FarmSim.Application.Simulation.Time.Contracts
{
    public interface ISimulationTimeDeltaSource
    {
        /// <summary>
        /// Delta time (seconds) to be consumed by simulation tick.
        /// Authoritative, read-only.
        /// </summary>
        float DeltaTimeSeconds { get; }
    }
}
