namespace FarmSim.Application.Simulation.World.Model.Biology
{
    /// <summary>
    /// Tier-1 world-scale biology aggregate root.
    /// Structural declaration only: inert, no logic, no ticking, no time binding, no read models,
    /// no diagnostics, no mutation/exposure.
    /// </summary>
    public sealed class SimulationWorldBiologyStateRoot
    {
        // Default constructor for structural aggregation (no logic, no ticking).
        public SimulationWorldBiologyStateRoot()
        {
        }
    }
}
