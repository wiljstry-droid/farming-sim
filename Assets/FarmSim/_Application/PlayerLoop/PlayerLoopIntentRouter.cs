using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace FarmSim.Application.PlayerLoop
{
    /// <summary>
    /// Phase A: minimal player-loop input router/dispatcher.
    /// Definition of done:
    /// - In Play Mode, keys 1/2/3 produce one clean proof line each (no spam).
    /// - Single router/dispatcher (this component) handles all three intents.
    /// </summary>
    public sealed class PlayerLoopIntentRouter : MonoBehaviour
    {
        private enum IntentKind
        {
            Plant = 1,
            Observe = 2,
            Modify = 3
        }

        private void Awake()
        {
            // Single proof line on startup. No spam.
            Debug.Log("[PlayerLoop] Ready (press 1=Plant, 2=Observe, 3=Modify)");
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.digit1Key.wasPressedThisFrame) Dispatch(IntentKind.Plant);
            if (keyboard.digit2Key.wasPressedThisFrame) Dispatch(IntentKind.Observe);
            if (keyboard.digit3Key.wasPressedThisFrame) Dispatch(IntentKind.Modify);
#else
            if (Input.GetKeyDown(KeyCode.Alpha1)) Dispatch(IntentKind.Plant);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Dispatch(IntentKind.Observe);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Dispatch(IntentKind.Modify);
#endif
        }

        private static void Dispatch(IntentKind intent)
        {
            Debug.Log($"[PlayerLoop] Intent={intent}");
        }
    }
}
