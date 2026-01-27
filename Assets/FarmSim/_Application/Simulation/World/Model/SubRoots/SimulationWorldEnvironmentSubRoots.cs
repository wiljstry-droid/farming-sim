namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Typed container for Tier-1 Environment sub-roots.
    /// Inert, non-behavioral, non-mutable declarations only.
    /// </summary>
    public readonly struct SimulationWorldEnvironmentSubRoots
    {
        public SimulationWorldEnvironmentWeatherStateRoot Weather { get; }
        public SimulationWorldEnvironmentClimateStateRoot Climate { get; }
        public SimulationWorldEnvironmentLightStateRoot Light { get; }
        public SimulationWorldEnvironmentEventsStateRoot Events { get; }

        public SimulationWorldEnvironmentSubRoots(
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

        public static SimulationWorldEnvironmentSubRoots CreateEmpty()
        {
            return new SimulationWorldEnvironmentSubRoots(
                default,
                default,
                default,
                default);
        }
    }
}
