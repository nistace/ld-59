using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LD59.ExtractMoles.Cutscenes
{
   public class CutsceneScene : MonoBehaviour
   {
      [SerializeField] private CutsceneSharedSettings _settings;
      [SerializeField] private CutscenePuppet[] _puppets;
      [SerializeField] private Transform[] _positions;

      public async UniTask MoveCharacter( CutsceneScript.CharacterChange change )
      {
         var puppet = _puppets[ (int)change.Character ];
         var position = _positions[ (int)change.CharacterPosition ];

         while(puppet.transform.position != position.position || puppet.transform.forward != position.forward)
         {
            puppet.transform.position = Vector3.MoveTowards( puppet.transform.position, position.position, _settings.PuppetsMovementSpeed * Time.deltaTime );
            puppet.transform.forward = Vector3.Slerp( puppet.transform.forward, position.forward, _settings.PuppetsMovementSpeed * Time.deltaTime );

            await UniTask.NextFrame();
         }

         puppet.transform.position = position.position;
         puppet.transform.forward = position.forward;
      }

      public void SnapCharacter( CutsceneScript.CharacterChange change )
      {
         var puppet = _puppets[ (int)change.Character ];
         var position = _positions[ (int)change.CharacterPosition ];

         puppet.transform.position = position.position;
      }

      public void SetAnimation( CutsceneCharacter character, CutscenePuppet.Animation animationState ) => _puppets[ (int)character ].SetAnimation( animationState );
   }
}