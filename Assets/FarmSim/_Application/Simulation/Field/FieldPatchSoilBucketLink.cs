using UnityEngine;
using FarmSim.Application.Simulation.Step;

namespace FarmSim.Application.Simulation.Field
{
    /// <summary>
    /// FieldPatchSoilBucketLink v1
    /// Non-invasive reference link: patch -> existing soil bucket step executor.
    /// No ownership, no simulation behavior changes.
    /// </summary>
    public sealed class FieldPatchSoilBucketLink : MonoBehaviour
    {
        [SerializeField]
        private FieldPatchAnchor patchAnchor;

        [SerializeField]
        private SoilMoistureBucketStepExecutor soilBucketStepExecutor;

        public FieldPatchAnchor PatchAnchor => patchAnchor;
        public SoilMoistureBucketStepExecutor SoilBucketStepExecutor => soilBucketStepExecutor;

        private void Awake()
        {
            if (patchAnchor == null)
                Debug.LogError("[FieldPatchSoilBucketLink] Missing PatchAnchor reference.", this);

            if (soilBucketStepExecutor == null)
                Debug.LogError("[FieldPatchSoilBucketLink] Missing SoilMoistureBucketStepExecutor reference.", this);
        }
    }
}
