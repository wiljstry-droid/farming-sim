using FarmSim.Application.Simulation.Field.Model;
using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Runtime probe to validate the FieldPatchActionGateAuthority can make decisions.
    /// This is diagnostic-only and does not mutate anything.
    ///
    /// It waits until the provider has a non-null gate before running once.
    /// </summary>
    public sealed class FieldPatchActionGateConsumerProbe : MonoBehaviour
    {
        [SerializeField]
        private FieldPatchActionGateAuthorityProvider provider;

        [Header("Probe Input")]
        [SerializeField]
        private string actingVesselId = "probe.vessel";

        [SerializeField]
        private int patchX = 0;

        [SerializeField]
        private int patchY = 0;

        [SerializeField]
        private FieldPatchActionKind actionKind = FieldPatchActionKind.SoilOperation;

        private bool _ran;

        private void Update()
        {
            if (_ran)
                return;

            if (provider == null)
            {
                Debug.LogError("[FieldPatchActionGateProbe] provider not assigned.");
                _ran = true;
                return;
            }

            var gate = provider.Gate;
            if (gate == null)
            {
                // Provider contract should never allow this now, but keep it defensive.
                return;
            }

            var request = new FieldPatchActionGateRequest(
                actionKind,
                new FieldVesselId(actingVesselId),
                new FieldPatchCoord(patchX, patchY)
            );

            var decision = gate.Decide(in request);

            Debug.Log(
                "[FieldPatchActionGateProbe] decision=" + decision.code +
                " allowed=" + decision.isAllowed +
                " reasons=" + decision.reasons +
                " actingVessel=" + actingVesselId +
                " patch=(" + patchX + "," + patchY + ")" +
                " currentOwner=" + decision.currentOwner.value
            );

            _ran = true;
        }
    }
}
