using System;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline.Contracts
{
    /// <summary>
    /// Canonical pipeline ordering rule:
    /// Primary: StepId (ordinal, lexicographic)
    /// Tie-breaker: Type full name (ordinal)
    /// </summary>
    public sealed class LexicographicStepIdOrderingPolicy : ISimulationStepOrderingPolicy
    {
        public int Compare(ISimulationStep a, ISimulationStep b)
        {
            if (ReferenceEquals(a, b)) return 0;
            if (a == null) return -1;
            if (b == null) return 1;

            var c = string.CompareOrdinal(a.StepId, b.StepId);
            if (c != 0) return c;

            var at = a.GetType().FullName ?? a.GetType().Name;
            var bt = b.GetType().FullName ?? b.GetType().Name;

            return string.CompareOrdinal(at, bt);
        }
    }
}
