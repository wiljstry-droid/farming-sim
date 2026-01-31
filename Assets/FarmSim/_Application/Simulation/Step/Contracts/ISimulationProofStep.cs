using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// Marker: this step is proof/validation only.
    /// Must be removable without affecting simulation correctness.
    /// </summary>
    public interface ISimulationProofStep : ISimulationStep
    {
    }
}
