namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Marker for declared authority dependencies.
    /// Used for diagnostics and future validation tooling.
    /// </summary>
    public interface IAuthorityDependency
    {
        string DependencyName { get; }
        bool IsSatisfied { get; }
    }
}
