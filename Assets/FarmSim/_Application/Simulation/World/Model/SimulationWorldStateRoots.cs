using FarmSim.Application.Simulation.World.Model.Biology;
using FarmSim.Application.Simulation.World.Model.Environment;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 world state aggregate roots (structural shells only).
    /// No logic, ticking, mutation, diagnostics, or exposure.
    /// </summary>
    public sealed class SimulationWorldStateRoots
    {
        public SimulationWorldLandStateRoot Land { get; }
        public SimulationWorldBiologyStateRoot Biology { get; }
        public SimulationWorldEnvironmentStateRoot Environment { get; }
        public SimulationWorldHumanStateRoot Human { get; }
        public SimulationWorldKnowledgeStateRoot Knowledge { get; }

        // Default constructor for structural aggregation (no logic, no ticking).
        public SimulationWorldStateRoots()
            : this(
                new SimulationWorldLandStateRoot(),
                new SimulationWorldBiologyStateRoot(),
                new SimulationWorldEnvironmentStateRoot(),
                new SimulationWorldHumanStateRoot(),
                new SimulationWorldKnowledgeStateRoot())
        {
        }

        public SimulationWorldStateRoots(
            SimulationWorldLandStateRoot land,
            SimulationWorldBiologyStateRoot biology,
            SimulationWorldEnvironmentStateRoot environment,
            SimulationWorldHumanStateRoot human,
            SimulationWorldKnowledgeStateRoot knowledge)
        {
            Land = land;
            Biology = biology;
            Environment = environment;
            Human = human;
            Knowledge = knowledge;
        }
    }

    // Structural shells (Tier-1 domains). No logic/ticking/mutation/exposure here.
    public sealed class SimulationWorldLandStateRoot
    {
        public SimulationWorldLandStateRoot() { }
    }

    public sealed class SimulationWorldHumanStateRoot
    {
        public SimulationWorldHumanStateRoot() { }
    }

    public sealed class SimulationWorldKnowledgeStateRoot
    {
        public SimulationWorldKnowledgeStateRoot() { }
    }
}
