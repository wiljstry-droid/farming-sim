using UnityEngine;
using FarmSim.Application.Bootstrap.Contracts;

namespace FarmSim.Application.Bootstrap.Diagnostics
{
    public sealed class BootstrapDiagnostics : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour bootstrapSource;

        private IBootstrapContract contract;

        private void Awake()
        {
            contract = bootstrapSource as IBootstrapContract;

            if (contract == null)
            {
                Debug.LogError(
                    "[BootstrapDiagnostics] Assigned source does not implement IBootstrapContract."
                );
            }
        }

        private void Update()
        {
            if (contract == null)
                return;

            if (!contract.IsReady)
            {
                Debug.LogWarning("[BootstrapDiagnostics] Bootstrap not ready yet.");
            }
        }
    }
}
