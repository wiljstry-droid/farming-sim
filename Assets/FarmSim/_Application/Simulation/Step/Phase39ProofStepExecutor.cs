#nullable enable

using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Tick.Contracts;
using FarmSim.Application.Simulation.Tick.Model;
using FarmSim.Application.Simulation.World;

namespace FarmSim.Application.Simulation.Step
{
    /// <summary>
    /// Phase 39: first non-debug, outcome-producing step (durable in-memory state change).
    /// IMPORTANT: This must execute via the step pipeline. Tick may call either interface entrypoint.
    /// </summary>
    public sealed class Phase39ProofStepExecutor : MonoBehaviour, ISimulationStep, ISimulationTickExecutor
    {
        [Header("Tier-1 State Target (Durable / Inspectable)")]
        [SerializeField] private SoilMoistureBucketWorldStateAuthority? _soilMoisture;

        public string StepId => "39.Phase39ProofStep";

        // Step pipeline entry point
        public void Execute(ISimulationStepContext context)
        {
            ExecuteCore((float)context.DeltaSeconds);
        }

        // Tick execution entry point (if Tick binds directly to tick executors)
        public void Execute(in SimulationTickSnapshot snapshot)
        {
            ExecuteCore(snapshot.deltaSeconds);
        }

        private void ExecuteCore(float deltaSeconds)
        {
            if (_soilMoisture == null)
            {
                Debug.LogError("[Phase39ProofStep] Missing SoilMoistureBucketWorldStateAuthority reference.");
                return;
            }

            if (deltaSeconds <= 0f) return;

            // Durable, inspectable state change (in-memory Tier-1 state slot)
            _soilMoisture.AddMoisture01(0.01f);
        }
    }
}
