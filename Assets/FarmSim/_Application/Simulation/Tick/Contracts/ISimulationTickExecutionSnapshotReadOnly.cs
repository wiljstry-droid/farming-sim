using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick.Contracts
{
    /// <summary>
    /// Read-only observer contract for the most recent tick execution snapshot.
    /// Diagnostics and proof probes MUST depend on this interface (not on any authority singletons).
    /// </summary>
    public interface ISimulationTickExecutionSnapshotReadOnly
    {
        /// <summary>
        /// The most recent execution snapshot produced by the tick authority.
        /// May be null until the first tick executes.
        /// </summary>
        SimulationTickExecutionSnapshot LastExecutionSnapshot { get; }
    }
}
