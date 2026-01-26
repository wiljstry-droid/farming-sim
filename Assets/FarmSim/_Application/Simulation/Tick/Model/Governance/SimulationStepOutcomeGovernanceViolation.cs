namespace FarmSim.Application.Simulation.Tick.Model.Governance
{
    /// <summary>
    /// Immutable violation record emitted by outcome governance.
    /// Never throw from governance; let callers decide what to do.
    /// </summary>
    public sealed class SimulationStepOutcomeGovernanceViolation
    {
        public SimulationStepOutcomeGovernanceViolationKind Kind { get; }
        public string Detail { get; }

        public bool IsViolation => Kind != SimulationStepOutcomeGovernanceViolationKind.None;

        public SimulationStepOutcomeGovernanceViolation(
            SimulationStepOutcomeGovernanceViolationKind kind,
            string detail = null)
        {
            Kind = kind;
            Detail = detail;
        }

        public static SimulationStepOutcomeGovernanceViolation None()
            => new SimulationStepOutcomeGovernanceViolation(SimulationStepOutcomeGovernanceViolationKind.None);

        public override string ToString()
            => IsViolation ? $"{Kind}: {Detail}" : "None";
    }
}
