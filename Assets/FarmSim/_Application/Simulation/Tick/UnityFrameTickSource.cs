using FarmSim.Application.Simulation.Tick.Contracts;

namespace FarmSim.Application.Simulation.Tick
{
    public sealed class UnityFrameTickSource : ISimulationTickSource
    {
        public bool TryGetTickDeltaSeconds(out float deltaSeconds)
        {
            deltaSeconds = UnityEngine.Time.deltaTime;
            return deltaSeconds > 0f;
        }
    }
}
