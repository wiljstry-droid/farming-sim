using FarmSim.Application.Simulation.World.Model.Biology;
using FarmSim.Application.Simulation.World.Model.Environment;

namespace FarmSim.Application.Simulation.World.Contracts
{
    /// <summary>
    /// Provides access to Tier-1 simulation world state roots.
    /// Structural contract only — no logic, no ticking, no mutation.
    /// </summary>
    public interface ISimulationWorldStateRootProvider
    {
        SimulationWorldLandStateRoot Land { get; }
        SimulationWorldBiologyStateRoot Biology { get; }
        SimulationWorldEnvironmentStateRoot Environment { get; }
        SimulationWorldHumanStateRoot Human { get; }
        SimulationWorldKnowledgeStateRoot Knowledge { get; }
    }
}
