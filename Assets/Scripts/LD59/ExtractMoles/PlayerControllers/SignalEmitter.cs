using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Signals;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class SignalEmitter : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      [SerializeField] private PlayerInfo _playerInfo;
      [SerializeField] private InputActionReference _emitSignalActionReference;
      [SerializeField] private UnityEvent _signalEmitted;

      public static UnityEvent OnSignalEmitted { get; } = new();

      private void OnEnable() => _emitSignalActionReference.action.performed += HandleEmitSignalActionPerformed;
      private void OnDisable() => _emitSignalActionReference.action.performed -= HandleEmitSignalActionPerformed;

      private void HandleEmitSignalActionPerformed( InputAction.CallbackContext obj )
      {
         if(_playerInfo.LockedByInteraction) return;
         if(!_playerInfo.Can( PlayerInfo.PlayerActions.Whistle )) return;

         SignalSystem.EmitSignal();
         _signalEmitted.Invoke();
         OnSignalEmitted.Invoke();
      }

      public void OnDespawn() => enabled = false;
   }
}