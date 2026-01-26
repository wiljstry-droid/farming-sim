using System;
using System.Collections.Generic;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Pipeline.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline
{
    /// <summary>
    /// Registry that produces a deterministic, governance-checked ordering of simulation steps.
    /// Canonical ordering: lexicographic StepId (ordinal), tie-breaker by type name.
    /// </summary>
    public sealed class OrderedSimulationStepRegistry : ISimulationStepRegistry
    {
        private readonly List<ISimulationStep> _steps = new List<ISimulationStep>();
        private readonly ISimulationStepOrderingPolicy _orderingPolicy;

        public OrderedSimulationStepRegistry()
            : this(new LexicographicStepIdOrderingPolicy())
        {
        }

        public OrderedSimulationStepRegistry(ISimulationStepOrderingPolicy orderingPolicy)
        {
            _orderingPolicy = orderingPolicy ?? throw new ArgumentNullException(nameof(orderingPolicy));
        }

        public void Register(ISimulationStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            _steps.Add(step);
        }

        public IReadOnlyList<ISimulationStep> GetOrdered()
        {
            // Sort deterministically by policy.
            _steps.Sort(CompareSteps);

            // Enforce contract on the produced ordered list.
            SimulationStepPipelineContractValidator.ValidateOrThrow(_steps, _orderingPolicy);

            return _steps;
        }

        private int CompareSteps(ISimulationStep a, ISimulationStep b)
        {
            return _orderingPolicy.Compare(a, b);
        }
    }
}
