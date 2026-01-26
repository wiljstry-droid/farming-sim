using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Unity-facing provider that exposes IFieldPatchAssignmentReadOnly
    /// by adapting the FieldPatchAssignmentAuthorityHost.
    ///
    /// Binds late to respect bootstrap lifecycle.
    /// </summary>
    public sealed class FieldPatchAssignmentReadOnlyProvider : MonoBehaviour
    {
        [SerializeField]
        private FieldPatchAssignmentAuthorityHost authorityHost;

        private IFieldPatchAssignmentReadOnly _readOnly;
        private bool _bound;

        public IFieldPatchAssignmentReadOnly ReadOnly => _readOnly;

        private void Start()
        {
            TryBind();
        }

        private void TryBind()
        {
            if (_bound)
                return;

            if (authorityHost == null)
            {
                Debug.LogError("[FieldPatchAssignmentReadOnlyProvider] authorityHost not assigned.");
                return;
            }

            var authority = authorityHost.Authority;
            if (authority == null)
            {
                Debug.LogWarning("[FieldPatchAssignmentReadOnlyProvider] Authority not yet available. Will retry.");
                return;
            }

            _readOnly = new FieldPatchAssignmentReadOnlyAdapter(authority);
            _bound = true;

            Debug.Log("[FieldPatchAssignmentReadOnlyProvider] Bound read-only adapter to assignment authority.");
        }

        private void Update()
        {
            if (!_bound)
                TryBind();
        }
    }
}
