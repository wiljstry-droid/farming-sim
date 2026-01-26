using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// FieldPatchAnchor v1
    /// Non-invasive spatial truth anchor.
    /// Holds identity only. No simulation authority.
    /// </summary>
    public sealed class FieldPatchAnchor : MonoBehaviour
    {
        [SerializeField]
        private string patchId = "FIELD_PATCH_001";

        public string PatchId => patchId;
    }
}
