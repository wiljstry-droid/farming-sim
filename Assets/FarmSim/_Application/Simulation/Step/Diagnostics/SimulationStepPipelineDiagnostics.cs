using System.Collections.Generic;
using System.Text;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Pipeline;

namespace FarmSim.Application.Simulation.Step.Diagnostics
{
    public static class SimulationStepPipelineDiagnostics
    {
        public static string BuildOrderedStepReport(ISimulationStepPipeline pipeline)
        {
            if (pipeline == null)
                return "[StepPipeline] <null pipeline>";

            IReadOnlyList<ISimulationStep> steps = pipeline.OrderedSteps;

            var sb = new StringBuilder();
            sb.AppendLine("[StepPipeline] Ordered Steps:");

            if (steps == null || steps.Count == 0)
            {
                sb.AppendLine("  (none)");
                return sb.ToString();
            }

            for (int i = 0; i < steps.Count; i++)
            {
                var s = steps[i];
                if (s == null)
                {
                    sb.AppendLine($"  {i:00}. <null>");
                    continue;
                }

                var t = s.GetType();
                var name = t.FullName ?? t.Name;

                var attr = (SimulationStepOrderAttribute)System.Attribute.GetCustomAttribute(
                    t,
                    typeof(SimulationStepOrderAttribute),
                    inherit: false
                );

                var order = attr?.Order ?? 0;
                sb.AppendLine($"  {i:00}. order={order} type={name}");
            }

            return sb.ToString();
        }
    }
}
