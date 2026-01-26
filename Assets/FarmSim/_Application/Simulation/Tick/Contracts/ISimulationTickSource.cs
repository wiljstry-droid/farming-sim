namespace FarmSim.Application.Simulation.Tick.Contracts
{
    public interface ISimulationTickSource
    {
        bool TryGetTickDeltaSeconds(out float deltaSeconds);
    }
}
