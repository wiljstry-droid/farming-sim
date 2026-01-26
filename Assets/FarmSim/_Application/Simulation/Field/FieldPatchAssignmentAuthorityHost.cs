using FarmSim.Application.Simulation.Field.Model;
using UnityEngine;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// Unity runtime host for the in-memory FieldPatchAssignmentAuthority.
    /// No persistence, no UI, no sim-step wiring. Just constructs and holds the authority.
    /// </summary>
    public sealed class FieldPatchAssignmentAuthorityHost : MonoBehaviour
    {
        private IFieldPatchAssignmentAuthority _authority;

        public IFieldPatchAssignmentAuthority Authority => _authority;

        private void Awake()
        {
            // Pure in-memory: start empty on each play session.
            var index = FieldPatchAssignmentDataFactory.CreateEmptyIndex();
            _authority = new FieldPatchAssignmentAuthority(index);
        }
    }
}
