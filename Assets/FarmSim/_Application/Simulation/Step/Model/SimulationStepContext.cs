using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Model
{
    /// <summary>
    /// Minimal immutable step context provided by the SimulationTickAuthority
    /// to each step during execution.
    /// </summary>
    public sealed class SimulationStepContext : ISimulationStepContext
    {
        public long Tick { get; }
        public double DeltaSeconds { get; }

        public SimulationStepContext(long tick, double deltaSeconds)
        {
            Tick = tick;
            DeltaSeconds = deltaSeconds;
        }
    }
}
