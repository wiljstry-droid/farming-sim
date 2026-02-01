using FarmSim.Application.Simulation.Intent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmSim.Application.PlayerLoop
{
    public sealed class PlayerLoopIntentRouter : MonoBehaviour
    {
        public enum IntentKind
        {
            Plant = 1,
            Observe = 2,
            Modify = 3
        }

        [SerializeField] private SimulationIntentDispatcher simulationIntentDispatcher;

        private void Awake()
        {
            // Phase B: keep wiring dead-simple and reliable.
            // If not assigned, auto-bind on the same GameObject.
            if (simulationIntentDispatcher == null)
            {
                simulationIntentDispatcher = GetComponent<SimulationIntentDispatcher>();
                if (simulationIntentDispatcher == null)
                {
                    simulationIntentDispatcher = gameObject.AddComponent<SimulationIntentDispatcher>();
                }
            }
        }

        private void Start()
        {
            Debug.Log("[PlayerLoop] Ready (press 1=Plant, 2=Observe, 3=Modify)");
        }

        private void Update()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.digit1Key.wasPressedThisFrame) Route(IntentKind.Plant);
            if (keyboard.digit2Key.wasPressedThisFrame) Route(IntentKind.Observe);
            if (keyboard.digit3Key.wasPressedThisFrame) Route(IntentKind.Modify);
        }

        private void Route(IntentKind kind)
        {
            var intent = kind.ToString();

            Debug.Log($"[PlayerLoop] Intent={intent}");
            simulationIntentDispatcher.Dispatch(intent);
        }
    }
}
