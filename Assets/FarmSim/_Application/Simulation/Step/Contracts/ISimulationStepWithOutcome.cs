using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Step.Contracts
{
    /// <summary>
    /// Optional opt-in contract for steps that want to report an explicit
    /// deterministic outcome (Success / Skipped / Denied / Failed) without
    /// relying on logs or exceptions.
    ///
    /// Legacy steps that only implement ISimulationStep remain supported.
    /// </summary>
    public interface ISimulationStepWithOutcome : ISimulationStep
    {
        SimulationStepOutcome ExecuteWithOutcome(ISimulationStepContext context);
    }
}
