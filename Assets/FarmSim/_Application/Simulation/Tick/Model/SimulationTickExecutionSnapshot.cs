using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Tick.Model
{
    /// <summary>
    /// Read-only snapshot of what happened during a single simulation tick.
    /// Captures tick identity, delta, and the ordered step outcomes.
    /// </summary>
    public sealed class SimulationTickExecutionSnapshot
    {
        public long TickIndex { get; }
        public float DeltaSeconds { get; }

        public IReadOnlyList<SimulationStepExecutionRecord> StepRecords { get; }

        public SimulationTickExecutionSnapshot(
            long tickIndex,
            float deltaSeconds,
            IReadOnlyList<SimulationStepExecutionRecord> stepRecords)
        {
            TickIndex = tickIndex;
            DeltaSeconds = deltaSeconds;
            StepRecords = stepRecords;
        }
    }
}
