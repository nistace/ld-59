using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class SignelEmittedEvent : MonoBehaviour
   {
      [SerializeField] private UnityEvent _signalEmitted = new();

      private void Start() => SignalEmitter.OnSignalEmitted.AddListener( HandleSignalEmitted );
      private void OnDestroy() => SignalEmitter.OnSignalEmitted.RemoveListener( HandleSignalEmitted );

      private void HandleSignalEmitted() => _signalEmitted.Invoke();
   }
}