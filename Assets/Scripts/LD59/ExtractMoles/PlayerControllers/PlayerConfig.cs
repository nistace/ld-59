using System;
using UnityEngine;

namespace LD59.ExtractMoles.PlayerControllers
{
   [CreateAssetMenu]
   public class PlayerConfig : ScriptableObject
   {
      [SerializeField] private MovementData _movement = new();
      [SerializeField] private StoneThrowingData _stoneThrowing = new();
      [SerializeField] private InteractionData _interactions = new();

      public MovementData Movement => _movement;
      public StoneThrowingData StoneThrowing => _stoneThrowing;
      public InteractionData Interactions => _interactions;

      [Serializable]
      public class MovementData
      {
         [SerializeField] private float _movementSpeed = 3;
         [SerializeField] private float _smoothForward = .1f;
         [SerializeField] private float _climbSpeed = 3;

         public float MovementSpeed => _movementSpeed;
         public float SmoothForward => _smoothForward;
         public float ClimbSpeed => _climbSpeed;
      }

      [Serializable]
      public class StoneThrowingData
      {
         [SerializeField] private float _stoneNoiseRadius = 3;
         [SerializeField] private float _stoneSpeed = 6;
         [SerializeField] private LayerMask _stoneHitMask = ~0;
         [SerializeField] private LayerMask _stoneSoundsLayerMask = ~0;

         public float StoneNoiseRadius => _stoneNoiseRadius;
         public float StoneSpeed => _stoneSpeed;
         public LayerMask StoneHitMask => _stoneHitMask;
         public LayerMask StoneSoundsLayerMask => _stoneSoundsLayerMask;
      }

      [Serializable]
      public class InteractionData
      {
         [SerializeField] private float _interactForwardOffset = .1f;
         [SerializeField] private float _interactionRadius = .8f;

         public float InteractForwardOffset => _interactForwardOffset;
         public float InteractionRadius => _interactionRadius;
      }
   }
}