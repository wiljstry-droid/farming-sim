#nullable enable

using UnityEngine;

namespace FarmSim.Application.Simulation.Step
{
    /// <summary>
    /// Phase 39: the smallest durable, inspectable piece of simulation state.
    /// Must be mutated only by a tick-driven step (no UI, no persistence).
    /// </summary>
    public sealed class Phase39ProofStateAuthority : MonoBehaviour
    {
        [Header("Phase 39 Proof State (Inspectable)")]
        [SerializeField] private long _revision;
        [SerializeField] private int _counter;

        public long Revision => _revision;
        public int Counter => _counter;

        /// <summary>
        /// Authoritative mutation entrypoint for Phase 39 proof.
        /// </summary>
        public void Increment()
        {
            _counter++;
            _revision++;
        }
    }
}
