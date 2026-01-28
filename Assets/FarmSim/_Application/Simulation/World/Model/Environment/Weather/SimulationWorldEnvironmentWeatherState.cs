namespace FarmSim.Application.Simulation.World.Model.Environment.Weather
{
    public sealed class SimulationWorldEnvironmentWeatherState
    {
        public readonly float AirTemperatureC;
        public readonly float RelativeHumidity01;
        public readonly float WindSpeedMetersPerSecond;
        public readonly float PrecipitationMillimeters;
        public readonly float CloudCover01;

        public SimulationWorldEnvironmentWeatherState(
            float airTemperatureC,
            float relativeHumidity01,
            float windSpeedMetersPerSecond,
            float precipitationMillimeters,
            float cloudCover01)
        {
            AirTemperatureC = airTemperatureC;
            RelativeHumidity01 = relativeHumidity01;
            WindSpeedMetersPerSecond = windSpeedMetersPerSecond;
            PrecipitationMillimeters = precipitationMillimeters;
            CloudCover01 = cloudCover01;
        }
    }
}
