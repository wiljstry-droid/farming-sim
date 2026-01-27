namespace FarmSim.Application.Simulation.Contracts
{
    /// <summary>
    /// Future-facing binder: central place to run authority Bind() deterministically.
    /// Not used yet in Phase 28.
    /// </summary>
    public interface IAuthorityBinder
    {
        AuthorityBindingReport BindOne(IAuthorityBindingContract authority);
    }
}
