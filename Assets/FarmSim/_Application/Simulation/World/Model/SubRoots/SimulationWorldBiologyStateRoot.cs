using FarmSim.Application.Simulation.World.Model.SubRoots;

namespace FarmSim.Application.Simulation.World.Model
{
    /// <summary>
    /// Tier-1 Biology domain root for simulation world state.
    /// </summary>
    public sealed class SimulationWorldBiologyStateRoot
    {
        public SimulationWorldBiologySystemStateRoot System { get; }

        public SimulationWorldBiologyStateRoot()
        {
            System = new SimulationWorldBiologySystemStateRoot();
        }
    }
}
