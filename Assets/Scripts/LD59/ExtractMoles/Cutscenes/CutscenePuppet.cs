using UnityEngine;

namespace LD59.ExtractMoles.Cutscenes
{
   public class CutscenePuppet : MonoBehaviour
   {
      private static readonly int TalkingAnimParam = Animator.StringToHash( "Talking" );
      private static readonly int ShrugAnimParam = Animator.StringToHash( "Shrug" );

      public enum Animation
      {
         Idle = 0,
         Talk = 1,
         Shrug = 2
      }

      [SerializeField] private Animator _animator;

      public void SetAnimation( Animation animationPhase )
      {
         if(animationPhase == Animation.Idle)
         {
            _animator.SetBool( TalkingAnimParam, false );
         }
         else if(animationPhase == Animation.Talk)
         {
            _animator.SetBool( TalkingAnimParam, true );
         }
         else if(animationPhase == Animation.Shrug)
         {
            _animator.SetTrigger( ShrugAnimParam );
         }
      }
   }
}