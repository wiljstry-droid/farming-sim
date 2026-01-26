using System.Collections.Generic;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline
{
    public interface ISimulationStepPipeline
    {
        IReadOnlyList<ISimulationStep> OrderedSteps { get; }
    }
}
