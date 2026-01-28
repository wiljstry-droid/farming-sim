using FarmSim.Application.Simulation.World.Model.Environment;
using UnityEngine;

namespace FarmSim.Application.Simulation.World
{
    public sealed class SimulationWorldStateAuthority : MonoBehaviour
    {
        public SimulationWorldEnvironmentStateRoot Environment { get; }

        public SimulationWorldStateAuthority()
        {
            Environment = new SimulationWorldEnvironmentStateRoot();
        }
    }
}
