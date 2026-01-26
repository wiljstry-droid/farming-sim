using FarmSim.Application.Simulation.Step.Model;

namespace FarmSim.Application.Simulation.Step
{
    public sealed class FirstSimulationStepCore
    {
        private readonly FirstSimulationStepState state = new FirstSimulationStepState();

        private double nextReportAtSeconds = 1d;

        public FirstSimulationStepState State => state;

        public bool Step(double deltaSeconds)
        {
            if (deltaSeconds <= 0d)
                return false;

            state.Advance(deltaSeconds);

            if (state.TotalSimSeconds >= nextReportAtSeconds)
            {
                nextReportAtSeconds = System.Math.Floor(state.TotalSimSeconds) + 1d;
                return true;
            }

            return false;
        }
    }
}
