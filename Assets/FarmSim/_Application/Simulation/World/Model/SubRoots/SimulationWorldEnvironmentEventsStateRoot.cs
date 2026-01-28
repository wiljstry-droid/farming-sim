using FarmSim.Application.Simulation.World.Model.Environment.Events;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 Environment domain sub-root (Events).
    /// Ownership container only. No behavior, no mutation, no exposure.
    /// </summary>
    public sealed class SimulationWorldEnvironmentEventsStateRoot
    {
        private readonly SimulationWorldEnvironmentEventsState _state;

        public SimulationWorldEnvironmentEventsStateRoot()
        {
            _state = null;
        }

        public SimulationWorldEnvironmentEventsStateRoot(SimulationWorldEnvironmentEventsState state)
        {
            _state = state;
        }
    }
}
