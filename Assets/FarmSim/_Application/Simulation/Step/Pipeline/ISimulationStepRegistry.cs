using System.Collections.Generic;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline
{
    public interface ISimulationStepRegistry
    {
        void Register(ISimulationStep step);
        IReadOnlyList<ISimulationStep> GetOrdered();
    }
}
