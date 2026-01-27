namespace FarmSim.Application.Simulation.World.Contracts
{
    using FarmSim.Application.Simulation.World;

    /// <summary>
    /// Declares canonical Tier-1 world-state roots.
    /// Read-only access only. No mutation contracts in Phase 30.
    /// </summary>
    public interface ISimulationWorldStateRootProvider
    {
        SimulationWorldLandStateRoot Land { get; }
        SimulationWorldEnvironmentStateRoot Environment { get; }
        SimulationWorldBiologyStateRoot Biology { get; }
        SimulationWorldHumanStateRoot Human { get; }
        SimulationWorldKnowledgeStateRoot Knowledge { get; }
    }
}
