using System.Text;
using UnityEngine;
using FarmSim.Application.Simulation.Tick.Model.Governance;

namespace FarmSim.Application.Simulation.Tick.Diagnostics
{
    /// <summary>
    /// Read-only governance validator for step outcomes.
    /// Inspects SimulationTickAuthority.Instance.LastExecutionSnapshot.
    /// </summary>
    public sealed class SimulationStepOutcomeGovernanceDebugProbe : MonoBehaviour
    {
        private long _lastSeenTickIndex = -1;

        private void Update()
        {
            var authority = SimulationTickAuthority.Instance;
            if (authority == null)
                return;

            var snapshot = authority.LastExecutionSnapshot;
            if (snapshot == null)
                return;

            if (snapshot.TickIndex == _lastSeenTickIndex)
                return;

            _lastSeenTickIndex = snapshot.TickIndex;

            var records = snapshot.StepRecords;
            if (records == null || records.Count == 0)
                return;

            int violations = 0;
            var sb = new StringBuilder(512);

            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];
                var outcome = record.Outcome;

                var violation = SimulationStepOutcomeGovernance.Validate(outcome);
                if (!violation.IsViolation)
                    continue;

                violations++;

                sb.Append("[OutcomeGov] tick=")
                  .Append(snapshot.TickIndex)
                  .Append(" step=")
                  .Append(record.Step == null ? "<null>" : record.Step.GetType().Name)
                  .Append(" outcome=")
                  .Append(outcome == null ? "<null>" : outcome.ToString())
                  .Append(" violation=")
                  .Append(violation.Kind)
                  .Append(" detail=")
                  .Append(violation.Detail)
                  .Append('\n');
            }

            if (violations > 0)
            {
                Debug.LogError(sb.ToString());
            }
        }
    }
}
