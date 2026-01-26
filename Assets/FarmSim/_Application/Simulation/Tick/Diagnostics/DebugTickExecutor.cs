using UnityEngine;
using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;

namespace FarmSim.Application.Simulation.Tick.Diagnostics
{
    public sealed class DebugTickExecutor : MonoBehaviour, ISimulationTickExecutor
    {
        private bool loggedOnce;
        private bool bound;

        private void Start()
        {
            TryBind();
        }

        private void Update()
        {
            if (!bound)
                TryBind();
        }

        private void TryBind()
        {
            var tick = FarmSim.Application.Simulation.Tick.SimulationTickAuthority.Instance;
            if (tick == null)
                return;

            tick.BindExecutor(this);
            bound = true;
        }

        public void Execute(in SimulationTickSnapshot snapshot)
        {
            if (loggedOnce)
                return;

            loggedOnce = true;
            Debug.Log($"[TickExec] First tick executed. dt={snapshot.deltaSeconds:0.000}s");
        }
    }
}
