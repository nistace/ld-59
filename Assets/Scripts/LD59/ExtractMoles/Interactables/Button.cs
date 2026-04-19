using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace LD59.ExtractMoles.Interactables
{
   [RequireComponent( typeof(StateHolder) )]
   public class Button : MonoBehaviour, IInteractable
   {
      [SerializeField] private Transform _interactionPoint;
      [SerializeField] private Transform _uiOverlayAnchorPoint;
      [SerializeField] private StateHolder _stateHolder;
      [SerializeField] private State[] _states;

      public UniTask Interact( Transform interactor )
      {
         _stateHolder.ChangeState( _states[ (Array.IndexOf( _states, _stateHolder.CurrentState ) + 1) % _states.Length ] );

         return UniTask.CompletedTask;
      }

      public Vector3 InteractionPoint => _interactionPoint.position;
      public Vector3 UIOverlayAnchorPoint => _uiOverlayAnchorPoint.position;
   }
}