namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Minimal declared dependency record (diagnostics-friendly).
    /// </summary>
    public sealed class AuthorityDependency : IAuthorityDependency
    {
        public string DependencyName { get; }
        public bool IsSatisfied { get; }

        public AuthorityDependency(string dependencyName, bool isSatisfied)
        {
            DependencyName = dependencyName ?? "<null>";
            IsSatisfied = isSatisfied;
        }
    }
}
