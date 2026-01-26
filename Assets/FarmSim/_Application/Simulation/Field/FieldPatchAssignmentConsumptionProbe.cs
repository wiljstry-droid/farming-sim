using UnityEngine;
using FarmSim.Application.Simulation.Field.Model;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Read-only consumer probe.
    /// Demonstrates safe consumption of FieldPatch assignment truth.
    /// No mutation. No simulation steps. Debug-only.
    /// </summary>
    public sealed class FieldPatchAssignmentConsumptionProbe : MonoBehaviour
    {
        [SerializeField]
        private FieldPatchAssignmentReadOnlyProvider readOnlyProvider;

        [SerializeField]
        private FieldVesselId actingVessel;

        [SerializeField]
        private FieldPatchCoord patch;

        private void Start()
        {
            if (readOnlyProvider == null || readOnlyProvider.ReadOnly == null)
            {
                Debug.LogWarning("[FieldPatchAssignmentConsumptionProbe] Read-only provider not available.");
                return;
            }

            var result = FieldPatchAssignmentConsumption.CheckEligibility(
                readOnlyProvider.ReadOnly,
                actingVessel,
                patch);

            Debug.Log(
                $"[FieldPatchAssignmentConsumptionProbe] " +
                $"eligible={result.isEligible} " +
                $"reason={result.reason} " +
                $"currentOwner={result.currentOwner}");
        }
    }
}
