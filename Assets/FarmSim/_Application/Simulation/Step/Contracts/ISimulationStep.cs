namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// A single deterministic unit of simulation work executed on a simulation tick.
    /// Engine-agnostic: no UnityEngine references allowed.
    /// </summary>
    public interface ISimulationStep
    {
        /// <summary>
        /// Stable identifier for diagnostics and ordering (e.g. "sim.step.weather").
        /// </summary>
        string StepId { get; }

        /// <summary>
        /// Executes the step. Must be deterministic given the provided context.
        /// </summary>
        void Execute(ISimulationStepContext context);
    }
}
