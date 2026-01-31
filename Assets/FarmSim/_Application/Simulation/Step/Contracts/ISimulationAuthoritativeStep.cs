using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// Marker: this step is part of the authoritative simulation tick spine.
    /// </summary>
    public interface ISimulationAuthoritativeStep : ISimulationStep
    {
    }
}
