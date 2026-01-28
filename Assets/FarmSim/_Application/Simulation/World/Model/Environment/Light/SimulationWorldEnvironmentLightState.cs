namespace FarmSim.Application.Simulation.World.Model.Environment.Light
{
    public sealed class SimulationWorldEnvironmentLightState
    {
        public readonly float DayLengthHours;
        public readonly float SolarIrradianceWattsPerSquareMeter;
        public readonly float LightQuality01;

        public SimulationWorldEnvironmentLightState(
            float dayLengthHours,
            float solarIrradianceWattsPerSquareMeter,
            float lightQuality01)
        {
            DayLengthHours = dayLengthHours;
            SolarIrradianceWattsPerSquareMeter = solarIrradianceWattsPerSquareMeter;
            LightQuality01 = lightQuality01;
        }
    }
}
