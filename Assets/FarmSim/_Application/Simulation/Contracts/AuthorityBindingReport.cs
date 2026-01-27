using System.Collections.Generic;

namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Data-only report of an authority binding attempt.
    /// Not wired yet. Intended for future bootstrap diagnostics and recon visibility.
    /// </summary>
    public sealed class AuthorityBindingReport
    {
        public string AuthorityName { get; }
        public bool Succeeded { get; }
        public IReadOnlyList<IAuthorityDependency> Dependencies { get; }
        public string FailureMessage { get; }

        public AuthorityBindingReport(
            string authorityName,
            bool succeeded,
            IReadOnlyList<IAuthorityDependency> dependencies,
            string failureMessage)
        {
            AuthorityName = authorityName ?? "<null>";
            Succeeded = succeeded;
            Dependencies = dependencies ?? new List<IAuthorityDependency>();
            FailureMessage = failureMessage;
        }

        public static AuthorityBindingReport Success(
            string authorityName,
            IReadOnlyList<IAuthorityDependency> dependencies)
            => new AuthorityBindingReport(authorityName, true, dependencies, null);

        public static AuthorityBindingReport Fail(
            string authorityName,
            IReadOnlyList<IAuthorityDependency> dependencies,
            string failureMessage)
            => new AuthorityBindingReport(authorityName, false, dependencies, failureMessage);
    }
}
