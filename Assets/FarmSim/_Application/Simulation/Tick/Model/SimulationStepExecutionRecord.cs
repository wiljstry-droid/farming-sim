using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Tick.Model
{
    /// <summary>
    /// Read-only record describing the execution of a single simulation step
    /// during a specific tick.
    /// </summary>
    public sealed class SimulationStepExecutionRecord
    {
        public ISimulationStep Step { get; }
        public SimulationStepOutcome Outcome { get; }

        public SimulationStepExecutionRecord(
            ISimulationStep step,
            SimulationStepOutcome outcome)
        {
            Step = step;
            Outcome = outcome;
        }
    }
}
