using UnityEngine;
using FarmSim.Application.Bootstrap.Contracts;

namespace FarmSim.Application.Bootstrap
{
    public sealed class BootstrapAuthority : MonoBehaviour, IBootstrapContract
    {
        public bool IsReady { get; private set; }

        private void Awake()
        {
            IsReady = false;
        }

        private void Start()
        {
            IsReady = true;
        }
    }
}
