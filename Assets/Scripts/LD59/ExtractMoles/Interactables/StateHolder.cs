using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Interactables
{
   public class StateHolder : MonoBehaviour
   {
      [SerializeField] private State _currentState;

      public State CurrentState => _currentState;
      public UnityEvent<State> OnStateChanged { get; } = new();

      public void ChangeState( State newState )
      {
         _currentState = newState;

         OnStateChanged.Invoke( _currentState );
      }

      public bool Is( State state ) => state == CurrentState;
   }
}