namespace FarmSim.Application.Simulation.Step.Pipeline.Contracts
{
    public enum SimulationStepPipelineContractViolationKind
    {
        None = 0,

        NullStep = 1,
        MissingStepId = 2,
        DuplicateStepId = 3,
        NonDeterministicOrder = 4,
    }
}
