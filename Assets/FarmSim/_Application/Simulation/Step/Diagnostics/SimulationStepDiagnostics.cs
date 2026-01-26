using System.Text;
using UnityEngine;
using FarmSim.Application.Simulation.Step.Contracts;
using FarmSim.Application.Simulation.Step.Pipeline;

namespace FarmSim.Application.Simulation.Step.Diagnostics
{
    public static class SimulationStepDiagnostics
    {
        public static void ReportPipeline(ISimulationStepPipeline pipeline)
        {
            if (pipeline == null)
            {
                Debug.LogWarning("[StepPipeline] ReportPipeline called with null pipeline.");
                return;
            }

            var steps = pipeline.OrderedSteps;
            if (steps == null)
            {
                Debug.LogWarning("[StepPipeline] Pipeline.OrderedSteps is null.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("[StepPipeline] Ordered Steps:");

            for (int i = 0; i < steps.Count; i++)
            {
                var step = steps[i];
                if (step == null)
                {
                    sb.AppendLine($"  {i:00}. stepId=<null-step> type=<null>");
                    continue;
                }

                sb.AppendLine(
                    $"  {i:00}. stepId={step.StepId} type={step.GetType().FullName}"
                );
            }

            Debug.Log(sb.ToString());
        }
    }
}
