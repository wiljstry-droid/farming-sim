namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 world-state sub-root declarations under Land.
    /// Inert, typed, non-behavioral, non-mutable.
    ///
    /// This does not wire into any authority or mutation pathway yet.
    /// It is purely structural scaffolding.
    /// </summary>
    public readonly struct SimulationWorldLandSubRoots
    {
        public readonly SimulationWorldLandFieldsStateRoot Fields;
        public readonly SimulationWorldLandSoilStateRoot Soil;
        public readonly SimulationWorldLandHydrologyStateRoot Hydrology;
        public readonly SimulationWorldLandHistoryStateRoot History;

        public SimulationWorldLandSubRoots(
            SimulationWorldLandFieldsStateRoot fields,
            SimulationWorldLandSoilStateRoot soil,
            SimulationWorldLandHydrologyStateRoot hydrology,
            SimulationWorldLandHistoryStateRoot history)
        {
            Fields = fields;
            Soil = soil;
            Hydrology = hydrology;
            History = history;
        }

        public static SimulationWorldLandSubRoots Default =>
            new SimulationWorldLandSubRoots(
                new SimulationWorldLandFieldsStateRoot(),
                new SimulationWorldLandSoilStateRoot(),
                new SimulationWorldLandHydrologyStateRoot(),
                new SimulationWorldLandHistoryStateRoot());
    }
}
