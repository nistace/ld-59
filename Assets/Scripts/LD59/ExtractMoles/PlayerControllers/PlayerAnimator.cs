using UnityEngine;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class PlayerAnimator : MonoBehaviour
   {
      private static readonly int SpeedNormalizedAnimParam = Animator.StringToHash( "SpeedNormalized" );

      [SerializeField] private Animator _animator;
      [SerializeField] private MovementController _movementController;

      private void Update()
      {
         _animator.SetFloat( SpeedNormalizedAnimParam, _movementController.Movement.magnitude );
      }
   }
}