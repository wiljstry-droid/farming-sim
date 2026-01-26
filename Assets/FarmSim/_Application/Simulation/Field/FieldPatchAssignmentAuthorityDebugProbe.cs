using FarmSim.Application.Simulation.Field.Model;
using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Minimal runtime proof that the authority exists and can mutate data in-memory.
    /// Logs once on Start. No UI/HUD. No sim-step integration.
    /// </summary>
    public sealed class FieldPatchAssignmentAuthorityDebugProbe : MonoBehaviour
    {
        [SerializeField] private FieldPatchAssignmentAuthorityHost host;

        private void Awake()
        {
            if (host == null)
            {
                host = GetComponentInParent<FieldPatchAssignmentAuthorityHost>();
            }

            Debug.Log(host != null
                ? "[FieldPatchAssign] DebugProbe Awake: host bound."
                : "[FieldPatchAssign] DebugProbe Awake: host NOT found.");
        }

        private void Start()
        {
            if (host == null)
                return;

            var authority = host.Authority;
            if (authority == null)
            {
                Debug.Log("[FieldPatchAssign] DebugProbe Start: authority NOT found.");
                return;
            }

            // Smoke test: assign patch -> read owner -> unassign -> confirm cleared.
            var vessel = new FieldVesselId("Vessel.Debug");
            var patch = new FieldPatchCoord(0, 0);

            authority.Assign(vessel, patch, utcSeconds: 100);

            var hasOwner = authority.TryGetActiveOwner(patch, out var owner);
            Debug.Log(hasOwner
                ? $"[FieldPatchAssign] After Assign: patch(0,0) owner={owner.value}"
                : "[FieldPatchAssign] After Assign: patch(0,0) owner=<none>");

            authority.Unassign(patch, utcSeconds: 200);

            hasOwner = authority.TryGetActiveOwner(patch, out owner);
            Debug.Log(hasOwner
                ? $"[FieldPatchAssign] After Unassign: patch(0,0) owner={owner.value}"
                : "[FieldPatchAssign] After Unassign: patch(0,0) owner=<none>");
        }
    }
}
