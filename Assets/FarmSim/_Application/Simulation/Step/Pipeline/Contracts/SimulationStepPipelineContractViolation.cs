using System;
using FarmSim.Application.Simulation.Step.Contracts;

namespace FarmSim.Application.Simulation.Step.Pipeline.Contracts
{
    public readonly struct SimulationStepPipelineContractViolation
    {
        public readonly SimulationStepPipelineContractViolationKind Kind;
        public readonly string Detail;
        public readonly string StepId;
        public readonly string StepType;

        public SimulationStepPipelineContractViolation(
            SimulationStepPipelineContractViolationKind kind,
            string detail,
            ISimulationStep step)
        {
            Kind = kind;
            Detail = detail ?? string.Empty;

            StepId = step?.StepId ?? string.Empty;
            StepType = step == null ? "<null>" : (step.GetType().FullName ?? step.GetType().Name);
        }

        public override string ToString()
        {
            return $"{Kind}: stepId='{StepId}' type='{StepType}' detail='{Detail}'";
        }
    }
}
