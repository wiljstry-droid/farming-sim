using System;
using System.Collections.Generic;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline.Contracts
{
    /// <summary>
    /// Pipeline contract enforcement for:
    /// - what qualifies as a step (non-null, StepId required)
    /// - valid ordering (deterministic, policy-defined)
    /// - uniqueness (StepId unique)
    /// </summary>
    public static class SimulationStepPipelineContractValidator
    {
        public static void ValidateOrThrow(
            IReadOnlyList<ISimulationStep> stepsInOrder,
            ISimulationStepOrderingPolicy orderingPolicy)
        {
            var violations = Validate(stepsInOrder, orderingPolicy);

            if (violations.Count == 0)
                return;

            throw new InvalidOperationException(BuildExceptionMessage(violations));
        }

        public static List<SimulationStepPipelineContractViolation> Validate(
            IReadOnlyList<ISimulationStep> stepsInOrder,
            ISimulationStepOrderingPolicy orderingPolicy)
        {
            if (stepsInOrder == null)
                throw new ArgumentNullException(nameof(stepsInOrder));

            if (orderingPolicy == null)
                throw new ArgumentNullException(nameof(orderingPolicy));

            var violations = new List<SimulationStepPipelineContractViolation>();
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            // Step validity + StepId uniqueness
            for (int i = 0; i < stepsInOrder.Count; i++)
            {
                var s = stepsInOrder[i];

                if (s == null)
                {
                    violations.Add(new SimulationStepPipelineContractViolation(
                        SimulationStepPipelineContractViolationKind.NullStep,
                        $"Index {i} is null.",
                        null
                    ));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(s.StepId))
                {
                    violations.Add(new SimulationStepPipelineContractViolation(
                        SimulationStepPipelineContractViolationKind.MissingStepId,
                        $"Index {i} has missing/blank StepId.",
                        s
                    ));
                }
                else
                {
                    if (!seenIds.Add(s.StepId))
                    {
                        violations.Add(new SimulationStepPipelineContractViolation(
                            SimulationStepPipelineContractViolationKind.DuplicateStepId,
                            $"Duplicate StepId detected at index {i}.",
                            s
                        ));
                    }
                }
            }

            // Ordering validity (must match policy ordering)
            // If there are null steps, ordering check still runs but may be noisy; that’s fine for contract enforcement.
            for (int i = 1; i < stepsInOrder.Count; i++)
            {
                var prev = stepsInOrder[i - 1];
                var curr = stepsInOrder[i];

                var c = orderingPolicy.Compare(prev, curr);

                if (c > 0)
                {
                    violations.Add(new SimulationStepPipelineContractViolation(
                        SimulationStepPipelineContractViolationKind.NonDeterministicOrder,
                        $"Ordering violated at indices {i - 1}->{i}. Policy says '{prev?.StepId}' must not come after '{curr?.StepId}'.",
                        curr
                    ));
                }
            }

            return violations;
        }

        private static string BuildExceptionMessage(List<SimulationStepPipelineContractViolation> violations)
        {
            var msg = "Simulation step pipeline contract violated:\n";
            for (int i = 0; i < violations.Count; i++)
                msg += "- " + violations[i] + "\n";
            return msg;
        }
    }
}
