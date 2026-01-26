using System;
using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Pure data model representing a user-defined field (FieldVessel).
    /// Spans many FieldPatches but owns no simulation behavior.
    /// </summary>
    [Serializable]
    public sealed class FieldVesselData
    {
        public FieldVesselId id;

        /// <summary>
        /// Human-readable label (user-defined).
        /// </summary>
        public string label;

        /// <summary>
        /// Lifecycle state of this field.
        /// </summary>
        public FieldVesselLifecycleState lifecycleState;

        /// <summary>
        /// UTC creation timestamp (seconds since epoch).
        /// </summary>
        public long createdUtcSeconds;

        /// <summary>
        /// UTC destruction timestamp (seconds since epoch), if destroyed.
        /// </summary>
        public long? destroyedUtcSeconds;

        /// <summary>
        /// Free-form metadata for future systems (tags, notes, etc.).
        /// Must not be interpreted by simulation logic at this stage.
        /// </summary>
        public Dictionary<string, string> metadata;

        public FieldVesselData(
            FieldVesselId id,
            string label,
            FieldVesselLifecycleState lifecycleState,
            long createdUtcSeconds)
        {
            this.id = id;
            this.label = label ?? string.Empty;
            this.lifecycleState = lifecycleState;
            this.createdUtcSeconds = createdUtcSeconds;
            this.destroyedUtcSeconds = null;
            this.metadata = new Dictionary<string, string>();
        }
    }
}
