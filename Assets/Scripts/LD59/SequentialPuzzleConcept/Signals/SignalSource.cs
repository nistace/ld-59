using UnityEngine;
using UnityEngine.Events;

namespace LD59.SequentialPuzzleConcept.Signals
{
   public class SignalSource : MonoBehaviour
   {
      [SerializeField] private string _signalName;
      [SerializeField] private bool _playerCanReact = true;

      public string SignalName => _signalName;
      public bool PlayerCanReact => _playerCanReact;

      public static UnityEvent<SignalSource> OnSignalEmitted { get; } = new();

      public void Emit() => OnSignalEmitted.Invoke( this );
   }
}