namespace FarmSim.Application.Simulation.Field.Model
{
    /// <summary>
    /// Lifecycle state of a FieldVessel.
    /// Data-only enum. No behavior.
    /// </summary>
    public enum FieldVesselLifecycleState
    {
        Unknown = 0,

        /// <summary>
        /// Field exists but is not currently active for planting.
        /// </summary>
        Inactive = 10,

        /// <summary>
        /// Field is active and may receive assignments.
        /// </summary>
        Active = 20,

        /// <summary>
        /// Field has been explicitly destroyed.
        /// History preserved; no further assignments allowed.
        /// </summary>
        Destroyed = 90
    }
}
