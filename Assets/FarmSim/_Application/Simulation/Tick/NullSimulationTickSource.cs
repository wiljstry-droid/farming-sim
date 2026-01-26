using FarmSim.Application.Simulation.Tick.Contracts;

namespace FarmSim.Application.Simulation.Tick
{
    public sealed class NullSimulationTickSource : ISimulationTickSource
    {
        public bool TryGetTickDeltaSeconds(out float deltaSeconds)
        {
            deltaSeconds = 0f;
            return false;
        }
    }
}
