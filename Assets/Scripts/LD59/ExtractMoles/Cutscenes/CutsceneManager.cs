using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.Cutscenes
{
   public class CutsceneManager : MonoBehaviour
   {
      [SerializeField] private CutsceneSharedSettings _settings;
      [SerializeField] private CutsceneScene _cutsceneScene;
      [SerializeField] private CutsceneTextManager _textManager;
      [SerializeField] private GameObject _cutsceneGameObject;
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private AudioSource _audioSource;
      [SerializeField] private bool _skipInEditor;
      [SerializeField] private InputActionReference _skipActionReference;

      public UnityEvent<CutsceneEvent> OnActionRequired { get; } = new();

      private bool Skipping { get; set; }

      private void Start()
      {
         _skipActionReference.action.performed += HandleSkip;
      }

      private void HandleSkip( InputAction.CallbackContext obj )
      {
         Skipping = true;

         if(_audioSource.isPlaying)
         {
            _audioSource.Stop();
         }
      }

      public async UniTask PlayCutscene( CutsceneScript script )
      {
         _skipActionReference.action.Enable();

         var skip = false;
#if UNITY_EDITOR
         skip = _skipInEditor;
#endif

         foreach(var initialCharacter in script.InitialCharacterPositions)
         {
            _cutsceneScene.SnapCharacter( initialCharacter );
         }

         _textManager.CleanUp();

         if(!skip)
         {
            await UniTask.NextFrame();

            await UniTask.WaitForSeconds( script.DelayBefore );
            _cutsceneGameObject.SetActive( true );

            for(var fadeProgress = 0f; fadeProgress < 1; fadeProgress += Time.deltaTime / _settings.BoxFadeInDuration)
            {
               _canvasGroup.alpha = fadeProgress;
               await UniTask.NextFrame();
            }

            _canvasGroup.alpha = 1;
         }

         foreach(var line in script.Lines)
         {
            if(line.EventBefore != CutsceneEvent.Type.None)
            {
               var lineEvent = new CutsceneEvent( line.EventBefore );
               OnActionRequired.Invoke( lineEvent );

               await UniTask.WaitUntil( () => lineEvent.Handled );
            }

            if(skip)
            {
               continue;
            }

            foreach(var characterChange in line.CharacterChanges)
            {
               var characterMovement = _cutsceneScene.MoveCharacter( characterChange );

               if(characterChange.Await)
               {
                  await characterMovement;
               }
            }

            if(Skipping)
            {
               Skipping = false;
            }
            else
            {
               await UniTask.WaitForSeconds( line.DelayBefore );
            }

            _cutsceneScene.SetAnimation( line.Character, CutscenePuppet.Animation.Talk );

            _audioSource.clip = line.Clip;
            _audioSource.Play();

            _textManager.ShowText( line.Text, _settings.CharacterColor( (int)line.Character ) );

            await UniTask.NextFrame();
            await UniTask.WaitWhile( () => _audioSource.isPlaying );

            _cutsceneScene.SetAnimation( line.Character, CutscenePuppet.Animation.Idle );

            if(Skipping)
            {
               continue;
            }

            if(line.EndWithAShrug)
            {
               _cutsceneScene.SetAnimation( line.Character, CutscenePuppet.Animation.Shrug );
            }

            _textManager.HideText();

            await UniTask.WaitForSeconds( line.DelayAfter );
         }

         if(!skip)
         {
            for(var fadeProgress = 0f; fadeProgress < 1; fadeProgress += Time.deltaTime / _settings.BoxFadeOutDuration)
            {
               _canvasGroup.alpha = 1 - fadeProgress;
               await UniTask.NextFrame();
            }
         }

         _canvasGroup.alpha = 0;

         _skipActionReference.action.Disable();
      }
   }
}