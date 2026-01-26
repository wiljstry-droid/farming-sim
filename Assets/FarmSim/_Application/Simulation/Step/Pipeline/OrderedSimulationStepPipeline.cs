using System;
using System.Collections.Generic;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Pipeline.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline
{
    /// <summary>
    /// Immutable pipeline snapshot (ordered step list).
    /// Contract validated at construction.
    /// </summary>
    public sealed class OrderedSimulationStepPipeline : ISimulationStepPipeline
    {
        private readonly List<ISimulationStep> _steps;
        private readonly ISimulationStepOrderingPolicy _orderingPolicy;

        public OrderedSimulationStepPipeline(IEnumerable<ISimulationStep> stepsInOrder)
            : this(stepsInOrder, new LexicographicStepIdOrderingPolicy())
        {
        }

        public OrderedSimulationStepPipeline(
            IEnumerable<ISimulationStep> stepsInOrder,
            ISimulationStepOrderingPolicy orderingPolicy)
        {
            if (stepsInOrder == null)
                throw new ArgumentNullException(nameof(stepsInOrder));

            _orderingPolicy = orderingPolicy ?? throw new ArgumentNullException(nameof(orderingPolicy));

            _steps = new List<ISimulationStep>();

            foreach (var step in stepsInOrder)
            {
                if (step == null)
                    throw new ArgumentException("Pipeline steps may not contain null entries.", nameof(stepsInOrder));

                _steps.Add(step);
            }

            // Governance: validate final ordered list.
            SimulationStepPipelineContractValidator.ValidateOrThrow(_steps, _orderingPolicy);
        }

        public IReadOnlyList<ISimulationStep> OrderedSteps => _steps;
    }
}
