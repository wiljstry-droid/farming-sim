namespace FarmSim.Application.Simulation.World.Model.Environment.Climate
{
    public sealed class SimulationWorldEnvironmentClimateState
    {
        public readonly float MeanAnnualTemperatureC;
        public readonly float AnnualRainfallMillimeters;
        public readonly float SeasonalVariability01;
        public readonly int KoppenClimateCode;

        public SimulationWorldEnvironmentClimateState(
            float meanAnnualTemperatureC,
            float annualRainfallMillimeters,
            float seasonalVariability01,
            int koppenClimateCode)
        {
            MeanAnnualTemperatureC = meanAnnualTemperatureC;
            AnnualRainfallMillimeters = annualRainfallMillimeters;
            SeasonalVariability01 = seasonalVariability01;
            KoppenClimateCode = koppenClimateCode;
        }
    }
}
