using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Interactables
{
   [RequireComponent( typeof(StateHolder) )]
   public class Button : MonoBehaviour, IInteractable, IReactToMoleHeadBump
   {
      [SerializeField] private Transform _interactionPoint;
      [SerializeField] private Transform _uiOverlayAnchorPoint;
      [SerializeField] private StateHolder _stateHolder;
      [SerializeField] private State[] _states;
      [SerializeField] private UnityEvent _pressed = new();

      public Vector3 InteractionPoint => _interactionPoint.position;
      public Vector3 UIOverlayAnchorPoint => _uiOverlayAnchorPoint.position;
      public float RequiredSpeedRatio => .5f;

      public UniTask Interact( Transform interactor )
      {
         ChangeToNextState();
         _pressed.Invoke();
         return UniTask.CompletedTask;
      }

      private void ChangeToNextState() => _stateHolder.ChangeState( _states[ (Array.IndexOf( _states, _stateHolder.CurrentState ) + 1) % _states.Length ] );

      public void React() => ChangeToNextState();
   }
}