using FarmSim.Application.Simulation.World.Model.Environment.Climate;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 Environment domain sub-root (Climate).
    /// Ownership container only. No behavior, no mutation, no exposure.
    /// </summary>
    public sealed class SimulationWorldEnvironmentClimateStateRoot
    {
        private readonly SimulationWorldEnvironmentClimateState _state;

        public SimulationWorldEnvironmentClimateStateRoot()
        {
            _state = null;
        }

        public SimulationWorldEnvironmentClimateStateRoot(SimulationWorldEnvironmentClimateState state)
        {
            _state = state;
        }
    }
}
