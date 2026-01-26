using System;

namespace FarmSim.Application.Simulation.Step.Contracts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class SimulationStepOrderAttribute : Attribute
    {
        public int Order { get; }

        public SimulationStepOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
