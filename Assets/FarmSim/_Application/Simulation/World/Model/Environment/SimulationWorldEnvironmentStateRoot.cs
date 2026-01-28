using FarmSim.Application.Simulation.World;

namespace FarmSim.Application.Simulation.World.Model.Environment
{
    public sealed class SimulationWorldEnvironmentStateRoot
    {
        public SimulationWorldEnvironmentWeatherStateRoot Weather { get; }
        public SimulationWorldEnvironmentClimateStateRoot Climate { get; }
        public SimulationWorldEnvironmentLightStateRoot Light { get; }
        public SimulationWorldEnvironmentEventsStateRoot Events { get; }

        // Default constructor for structural aggregation (no logic, no ticking).
        public SimulationWorldEnvironmentStateRoot()
            : this(
                new SimulationWorldEnvironmentWeatherStateRoot(),
                new SimulationWorldEnvironmentClimateStateRoot(),
                new SimulationWorldEnvironmentLightStateRoot(),
                new SimulationWorldEnvironmentEventsStateRoot())
        {
        }

        public SimulationWorldEnvironmentStateRoot(
            SimulationWorldEnvironmentWeatherStateRoot weather,
            SimulationWorldEnvironmentClimateStateRoot climate,
            SimulationWorldEnvironmentLightStateRoot light,
            SimulationWorldEnvironmentEventsStateRoot events)
        {
            Weather = weather;
            Climate = climate;
            Light = light;
            Events = events;
        }
    }
}
