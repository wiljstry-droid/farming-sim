namespace FarmSim.Application.Simulation.Step.Model
{
    /// <summary>
    /// Stable, machine-readable gate denial reason codes.
    /// Steps may define additional codes, but these are the canonical baseline.
    /// </summary>
    public static class SimulationStepGateReasonCodes
    {
        public const string Unspecified = "GateDenied.Unspecified";
        public const string MissingDependency = "GateDenied.MissingDependency";
        public const string NotReady = "GateDenied.NotReady";
        public const string InvalidState = "GateDenied.InvalidState";
        public const string ExternalAuthorityUnavailable = "GateDenied.ExternalAuthorityUnavailable";
        public const string PreconditionsNotMet = "GateDenied.PreconditionsNotMet";
    }
}
