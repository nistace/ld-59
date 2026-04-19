using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Signals;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class SignalEmitter : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      [SerializeField] private PlayerInfo _playerInfo;
      [SerializeField] private InputActionReference _emitSignalActionReference;

      private void OnEnable() => _emitSignalActionReference.action.performed += HandleEmitSignalActionPerformed;
      private void OnDisable() => _emitSignalActionReference.action.performed -= HandleEmitSignalActionPerformed;

      private void HandleEmitSignalActionPerformed( InputAction.CallbackContext obj )
      {
         if(_playerInfo.LockedByInteraction) return;

         SignalSystem.EmitSignal();
      }

      public void OnDespawn() => enabled = false;
   }
}