namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Declares how an authority binds its required dependencies.
    /// Binding must be explicit, validated, and deterministic.
    /// </summary>
    public interface IAuthorityBindingContract
    {
        /// <summary>
        /// Called once during bootstrap to bind required dependencies.
        /// Implementations must validate presence and fail loudly if invalid.
        /// </summary>
        void Bind();
    }
}
