namespace FarmSim.Application.Simulation.World.Model.Environment.Events
{
    public sealed class SimulationWorldEnvironmentEventsState
    {
        public readonly bool ExtremeEventActive;
        public readonly int ActiveEventCode;
        public readonly float EventSeverity01;

        public SimulationWorldEnvironmentEventsState(
            bool extremeEventActive,
            int activeEventCode,
            float eventSeverity01)
        {
            ExtremeEventActive = extremeEventActive;
            ActiveEventCode = activeEventCode;
            EventSeverity01 = eventSeverity01;
        }
    }
}
