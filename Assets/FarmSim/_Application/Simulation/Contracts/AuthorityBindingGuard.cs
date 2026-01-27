using System;

namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Canonical guard helpers for authority binding.
    /// Pure utilities: no Unity dependencies, no side effects unless called.
    /// </summary>
    public static class AuthorityBindingGuard
    {
        public static T RequireNotNull<T>(T value, string owner, string dependencyName)
            where T : class
        {
            if (value != null)
                return value;

            throw new AuthorityBindingException(
                $"[Bind] {owner} missing dependency: {dependencyName}"
            );
        }

        public static void RequireTrue(bool condition, string owner, string message)
        {
            if (condition)
                return;

            throw new AuthorityBindingException(
                $"[Bind] {owner} invalid state: {message}"
            );
        }

        public static void RequireOnce(bool alreadyBound, string owner)
        {
            if (!alreadyBound)
                return;

            throw new AuthorityBindingException(
                $"[Bind] {owner} Bind() called more than once."
            );
        }
    }
}
