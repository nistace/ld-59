using LD59.ExtractMoles.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class SignalEmitter : MonoBehaviour
   {
      [SerializeField] private InputActionReference _emitSignalActionReference;

      private void OnEnable() => _emitSignalActionReference.action.performed += HandleEmitSignalActionPerformed;
      private void OnDisable() => _emitSignalActionReference.action.performed -= HandleEmitSignalActionPerformed;

      private static void HandleEmitSignalActionPerformed( InputAction.CallbackContext obj ) => SignalSystem.EmitSignal();
   }
}