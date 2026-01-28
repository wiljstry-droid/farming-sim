using FarmSim.Application.Simulation.World.Model.Environment.Weather;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 Environment domain sub-root (Weather).
    /// Ownership container only. No behavior, no mutation, no exposure.
    /// </summary>
    public sealed class SimulationWorldEnvironmentWeatherStateRoot
    {
        private readonly SimulationWorldEnvironmentWeatherState _state;

        public SimulationWorldEnvironmentWeatherStateRoot()
        {
            _state = null;
        }

        public SimulationWorldEnvironmentWeatherStateRoot(SimulationWorldEnvironmentWeatherState state)
        {
            _state = state;
        }
    }
}
