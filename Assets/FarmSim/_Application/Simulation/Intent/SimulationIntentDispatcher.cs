using UnityEngine;

namespace FarmSim.Application.Simulation.Intent
{
    public sealed class SimulationIntentDispatcher : MonoBehaviour
    {
        /// <summary>
        /// Phase B stub: single entry point for Plant / Observe / Modify.
        /// Called by PlayerLoopIntentRouter.
        /// </summary>
        public void Dispatch(string intent)
        {
            Debug.Log($"[SimulationIntent] Received={intent}");
        }
    }
}
