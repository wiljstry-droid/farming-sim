using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline.Contracts
{
    /// <summary>
    /// Defines the single authoritative ordering rule for a simulation-step pipeline.
    /// This is governance, not gameplay.
    /// </summary>
    public interface ISimulationStepOrderingPolicy
    {
        /// <summary>
        /// Compares two steps for ordering. Must be deterministic and stable.
        /// </summary>
        int Compare(ISimulationStep a, ISimulationStep b);
    }
}
