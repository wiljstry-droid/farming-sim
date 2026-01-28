using FarmSim.Application.Simulation.World.Model.Environment.Light;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 Environment domain sub-root (Light).
    /// Ownership container only. No behavior, no mutation, no exposure.
    /// </summary>
    public sealed class SimulationWorldEnvironmentLightStateRoot
    {
        private readonly SimulationWorldEnvironmentLightState _state;

        public SimulationWorldEnvironmentLightStateRoot()
        {
            _state = null;
        }

        public SimulationWorldEnvironmentLightStateRoot(SimulationWorldEnvironmentLightState state)
        {
            _state = state;
        }
    }
}
