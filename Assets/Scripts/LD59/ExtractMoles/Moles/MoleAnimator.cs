using UnityEngine;

namespace LD59.ExtractMoles.Moles
{
   public class MoleAnimator : MonoBehaviour
   {
      private static readonly int BumpedAnimParam = Animator.StringToHash( "Bumped" );
      private static readonly int SpeedNormalizedAnimParam = Animator.StringToHash( "SpeedNormalized" );
      [SerializeField] private Animator _animator;
      [SerializeField] private MoleController _moleController;

      private void Update()
      {
         _animator.SetFloat( SpeedNormalizedAnimParam, _moleController.SpeedNormalized );
         _animator.SetBool( BumpedAnimParam, _moleController.IsKnockedOut );
      }
   }
}