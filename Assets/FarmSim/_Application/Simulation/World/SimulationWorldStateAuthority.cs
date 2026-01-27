namespace FarmSim.Application.Simulation.World
{
    using FarmSim.Application.Simulation.World.Contracts;
    using UnityEngine;

    /// <summary>
    /// Tier-1 authority declaring canonical world-state root containers.
    /// Phase 30: roots are empty typed shells only (no behavior, no mutation).
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SimulationWorldStateAuthority : MonoBehaviour, ISimulationWorldStateRootProvider
    {
        public static SimulationWorldStateAuthority Instance { get; private set; }

        public SimulationWorldLandStateRoot Land { get; private set; }
        public SimulationWorldEnvironmentStateRoot Environment { get; private set; }
        public SimulationWorldBiologyStateRoot Biology { get; private set; }
        public SimulationWorldHumanStateRoot Human { get; private set; }
        public SimulationWorldKnowledgeStateRoot Knowledge { get; private set; }

        private bool initialized;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[WorldState] Duplicate SimulationWorldStateAuthority detected. Destroying the new instance.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            InitializeRootsIfNeeded();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void InitializeRootsIfNeeded()
        {
            if (initialized) return;

            // Phase 30: typed shells only. No mutation logic, no simulation coupling.
            Land = new SimulationWorldLandStateRoot();
            Environment = new SimulationWorldEnvironmentStateRoot();
            Biology = new SimulationWorldBiologyStateRoot();
            Human = new SimulationWorldHumanStateRoot();
            Knowledge = new SimulationWorldKnowledgeStateRoot();

            initialized = true;
        }
    }
}
