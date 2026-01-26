namespace FarmSim.Application.Simulation.Step.Model
{
    public sealed class FirstSimulationStepState
    {
        public ulong TotalTicks { get; private set; }
        public double TotalSimSeconds { get; private set; }

        public void Advance(double deltaSeconds)
        {
            if (deltaSeconds <= 0d)
                return;

            TotalTicks++;
            TotalSimSeconds += deltaSeconds;
        }
    }
}
