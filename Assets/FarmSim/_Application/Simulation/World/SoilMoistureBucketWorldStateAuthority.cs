#nullable enable

using UnityEngine;

namespace FarmSim.Application.Simulation.World
{
    /// <summary>
    /// Tier-1 durable, inspectable in-memory state slot for Phase 39.
    /// MUST only be mutated by a tick-executed, gate-governed step.
    /// </summary>
    public sealed class SoilMoistureBucketWorldStateAuthority : MonoBehaviour
    {
        [Header("Soil Moisture Bucket (0..1)")]
        [SerializeField, Range(0f, 1f)]
        private float _moisture01 = 0.25f;

        public float Moisture01 => _moisture01;

        /// <summary>
        /// INTERNAL mutation entrypoint.
        /// Only simulation step executors should call this (via Tick -> Step -> Outcome).
        /// </summary>
        internal void SetMoisture01(float value01)
        {
            _moisture01 = Mathf.Clamp01(value01);
        }

        /// <summary>
        /// INTERNAL additive mutation entrypoint.
        /// Only simulation step executors should call this (via Tick -> Step -> Outcome).
        /// </summary>
        internal void AddMoisture01(float delta01)
        {
            _moisture01 = Mathf.Clamp01(_moisture01 + delta01);
        }
    }
}
