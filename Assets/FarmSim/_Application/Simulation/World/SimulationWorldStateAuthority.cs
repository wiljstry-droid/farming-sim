using UnityEngine;
using FarmSim.Application.Simulation.Contracts;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 root authority for simulation world state.
    ///
    /// This authority declares ownership of canonical world truth containers.
    /// It does not execute simulation logic, mutate data, or depend on Tick, UI, or Time.
    /// </summary>
    public sealed class SimulationWorldStateAuthority
        : MonoBehaviour, IAuthorityBindingContract
    {
        public static SimulationWorldStateAuthority Instance { get; private set; }

        private bool _isBound;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Bind is intentionally minimal.
        /// Declares this authority as the canonical owner of world state roots.
        /// No dependencies are required in Phase 29.
        /// </summary>
        public void Bind()
        {
            AuthorityBindingGuard.RequireOnce(_isBound, nameof(SimulationWorldStateAuthority));

            // Phase 29: no dependencies to bind.
            // This authority exists to define ownership boundaries only.

            _isBound = true;
        }
    }
}
