namespace FarmSim.Application.Simulation.Tick.Model
{
    public readonly struct SimulationTickSnapshot
    {
        public readonly float deltaSeconds;

        public SimulationTickSnapshot(float deltaSeconds)
        {
            this.deltaSeconds = deltaSeconds;
        }
    }
}
