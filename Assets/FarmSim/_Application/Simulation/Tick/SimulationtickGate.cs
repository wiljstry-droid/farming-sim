namespace FarmSim.Application.Simulation.Tick
{
    public sealed class SimulationTickGate
    {
        private bool _isPaused;

        public void SetPaused(bool paused)
        {
            _isPaused = paused;
        }

        public bool CanTick()
        {
            return !_isPaused;
        }
    }
}
