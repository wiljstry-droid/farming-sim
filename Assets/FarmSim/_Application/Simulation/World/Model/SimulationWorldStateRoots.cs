namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 root: canonical land truth container (typed shell only).
    /// Phase 31: declares inert sub-root shells (no behavior, no mutation, no coupling, no serialization).
    /// </summary>
    public sealed class SimulationWorldLandStateRoot
    {
        public SimulationWorldLandSubRoots SubRoots { get; }

        internal SimulationWorldLandStateRoot()
        {
            SubRoots = SimulationWorldLandSubRoots.Default;
        }
    }

    /// <summary>
    /// Tier-1 root: canonical environment truth container (typed shell only).
    /// </summary>
    public sealed class SimulationWorldEnvironmentStateRoot
    {
        internal SimulationWorldEnvironmentStateRoot() { }
    }

    /// <summary>
    /// Tier-1 root: canonical biology truth container (typed shell only).
    /// </summary>
    public sealed class SimulationWorldBiologyStateRoot
    {
        internal SimulationWorldBiologyStateRoot() { }
    }

    /// <summary>
    /// Tier-1 root: canonical human truth container (typed shell only).
    /// </summary>
    public sealed class SimulationWorldHumanStateRoot
    {
        internal SimulationWorldHumanStateRoot() { }
    }

    /// <summary>
    /// Tier-1 root: canonical knowledge truth container (typed shell only).
    /// </summary>
    public sealed class SimulationWorldKnowledgeStateRoot
    {
        internal SimulationWorldKnowledgeStateRoot() { }
    }
}
