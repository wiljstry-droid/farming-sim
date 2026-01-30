using FarmSim.Application.Simulation.World.Model.Biology;
using FarmSim.Application.Simulation.World.Model.Environment;

namespace FarmSim.Application.Simulation.World
{
    public sealed class SimulationWorldStateRoots
    {
        public SimulationWorldLandStateRoot Land { get; }
        public SimulationWorldEnvironmentStateRoot Environment { get; }
        public SimulationWorldHumanStateRoot Human { get; }
        public SimulationWorldKnowledgeStateRoot Knowledge { get; }
        public SimulationWorldBiologyStateRoot Biology { get; }

        public SimulationWorldStateRoots(
            SimulationWorldLandStateRoot land,
            SimulationWorldEnvironmentStateRoot environment,
            SimulationWorldHumanStateRoot human,
            SimulationWorldKnowledgeStateRoot knowledge,
            SimulationWorldBiologyStateRoot biology)
        {
            Land = land;
            Environment = environment;
            Human = human;
            Knowledge = knowledge;
            Biology = biology;
        }
    }

    public sealed class SimulationWorldLandStateRoot
    {
        public SimulationWorldLandStateRoot()
        {
        }
    }

    public sealed class SimulationWorldHumanStateRoot
    {
        public SimulationWorldHumanStateRoot()
        {
        }
    }

    public sealed class SimulationWorldKnowledgeStateRoot
    {
        public SimulationWorldKnowledgeStateRoot()
        {
        }
    }
}
