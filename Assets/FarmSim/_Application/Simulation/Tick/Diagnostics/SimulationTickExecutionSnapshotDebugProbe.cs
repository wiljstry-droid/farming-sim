using System.Text;
using UnityEngine;
using FarmSim.Application.Simulation.Tick;

namespace FarmSim.Application.Simulation.Tick.Diagnostics
{
    /// <summary>
    /// Prints the most recent SimulationTickExecutionSnapshot.
    /// Phase 24 acceptance requires Denied outcomes (with reason codes) to be visible here.
    /// </summary>
    public sealed class SimulationTickExecutionSnapshotDebugProbe : MonoBehaviour
    {
        [SerializeField] private bool printEveryTick = true;

        private long lastTickPrinted = -1;

        private void Update()
        {
            if (!printEveryTick)
                return;

            var authority = SimulationTickAuthority.Instance;
            if (authority == null)
                return;

            var snap = authority.LastExecutionSnapshot;
            if (snap == null)
                return;

            // Print once per tick index.
            if (snap.TickIndex == lastTickPrinted)
                return;

            lastTickPrinted = snap.TickIndex;

            var sb = new StringBuilder(1024);
            sb.Append("[TickExecSnap] tick=").Append(snap.TickIndex)
              .Append(" dt=").Append(snap.DeltaSeconds)
              .Append(" steps=").Append(snap.StepRecords.Count);

            for (int i = 0; i < snap.StepRecords.Count; i++)
            {
                var r = snap.StepRecords[i];
                var stepId = (r.Step != null) ? r.Step.StepId : "<null-step>";
                sb.Append("\n  - step=").Append(stepId)
                  .Append(" outcome=").Append(r.Outcome);
            }

            Debug.Log(sb.ToString());
        }
    }
}
