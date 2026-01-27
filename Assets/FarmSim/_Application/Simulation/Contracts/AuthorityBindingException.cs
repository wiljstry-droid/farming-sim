using System;

namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Thrown when an authority fails to bind its declared dependencies.
    /// This is a hard-fail by design.
    /// </summary>
    public sealed class AuthorityBindingException : Exception
    {
        public AuthorityBindingException(string message)
            : base(message)
        {
        }
    }
}
