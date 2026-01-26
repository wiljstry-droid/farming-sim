using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick
{
    public sealed class NullSimulationTickExecutor : ISimulationTickExecutor
    {
        public void Execute(in SimulationTickSnapshot snapshot)
        {
            // Intentionally no-op.
        }
    }
}
