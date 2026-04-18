using LD59.ExtractMoles.Interactables;
using System;
using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   [RequireComponent( typeof(StateHolder) )]
   public class BreakableWall : MonoBehaviour, IReactToMoleHeadBump
   {
      [SerializeField] private StateHolder _stateHolder;
      [SerializeField] private MeshFilter _meshFilter;
      [SerializeField] private StateData _breakableState;
      [SerializeField] private StateData _brokenState;
      [SerializeField] private float _requiredSpeedRatio = .5f;

      public float RequiredSpeedRatio => _requiredSpeedRatio;

      public void React()
      {
         if(!_stateHolder.Is( _breakableState.State )) return;

         _stateHolder.ChangeState( _brokenState.State );

         _meshFilter.sharedMesh = _brokenState.Visual;

         foreach(var stateCollider in _breakableState.Colliders)
         {
            stateCollider.enabled = false;
         }

         foreach(var stateCollider in _brokenState.Colliders)
         {
            stateCollider.enabled = true;
         }
      }

      [Serializable]
      private class StateData
      {
         [SerializeField] private State _state;
         [SerializeField] private Collider[] _colliders;
         [SerializeField] private Mesh _visual;

         public State State => _state;
         public Collider[] Colliders => _colliders;
         public Mesh Visual => _visual;
      }
   }
}