using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick.Contracts
{
    /// <summary>
    /// Receives authoritative tick snapshots for simulation execution.
    /// Engine-agnostic (no Unity types).
    /// </summary>
    public interface ISimulationTickExecutor
    {
        void Execute(in SimulationTickSnapshot snapshot);
    }
}
