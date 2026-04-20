using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Cutscenes;
using LD59.ExtractMoles.PlayerControllers;
using LD59.Levels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD59
{
   public class GameController : MonoBehaviour
   {
      [SerializeField] private LevelManager _levelManager;
      [SerializeField] private CutsceneScript _finalCutscene;
      [SerializeField] private CutsceneManager _cutsceneManager;

      private void Start()
      {
         _levelManager.OnCurrentLevelEnded.AddListener( HandleCurrentLevelEnded );
         _cutsceneManager.OnActionRequired.AddListener( HandleCutsceneActionRequired );
         CutsceneRequests.OnRequestReceived.AddListener( HandleCutsceneRequested );
         CharacterEvents.OnCharacterDied.AddListener( HandleCharacterDied );

         _levelManager.InstantiateFirstLevel();

         StartLevelAsync( true ).Forget();
      }

      private void HandleCutsceneRequested( ICutsceneRequester requester ) => HandleCutsceneRequestAsync( requester ).Forget();

      private async UniTask HandleCutsceneRequestAsync( ICutsceneRequester requester )
      {
         if(requester.RemoveAllPlayerActionsOnStart && PlayerInfo.PlayerInstance)
         {
            PlayerInfo.PlayerInstance.ActiveActions = 0;
         }

         await _cutsceneManager.PlayCutscene( requester.Script );

         if(PlayerInfo.PlayerInstance)
         {
            PlayerInfo.PlayerInstance.ActiveActions = requester.PlayerActionsAfterCutscene;
         }

         requester.OnCutsceneEnded();
      }

      private void HandleCharacterDied() => RestartLevelAsync().Forget();

      private async UniTask RestartLevelAsync()
      {
         _levelManager.InstantiateCurrentLevelAsNext();

         foreach(var character in FindObjectsByType<Character>( FindObjectsInactive.Exclude, FindObjectsSortMode.None ))
         {
            await character.Despawn();
         }

         await _levelManager.DespawnCurrentLevel();
         _levelManager.MoveNextLevelToCurrent();

         await StartLevelAsync( false );
      }

      private async UniTask StartLevelAsync( bool withCutscenes )
      {
         if(!_levelManager.CurrentLevel)
         {
            Debug.LogError( "No current level!" );
            return;
         }

         if(_levelManager.CurrentLevel.SpawnBeforeIntroCutscene)
         {
            await _levelManager.SpawnCurrentLevel();
         }

         if(withCutscenes && _levelManager.CurrentLevel.IntroScript)
         {
            await _cutsceneManager.PlayCutscene( _levelManager.CurrentLevel.IntroScript );
         }

         if(_levelManager.CurrentLevelState != LevelManager.LevelState.Spawned)
         {
            await _levelManager.SpawnCurrentLevel();
         }

         if(PlayerInfo.PlayerInstance)
         {
            PlayerInfo.PlayerInstance.ActiveActions |= _levelManager.CurrentLevel.ActiveActionsAfterCutscene;
         }

         _levelManager.CurrentLevel.StartLevel();
      }

      private async UniTask EndLevelAsync()
      {
         if(!_levelManager.CurrentLevel)
         {
            Debug.LogError( "No current level!" );
            return;
         }

         if(_levelManager.HasNextLevel())
         {
            _levelManager.InstantiateNextLevel();
         }

         if(_levelManager.CurrentLevelState != LevelManager.LevelState.Despawned)
         {
            _levelManager.SaveItemsToKeepBeforeNextLevel();
            await _levelManager.DespawnCurrentLevel();
         }

         if(_levelManager.HasNextLevel())
         {
            _levelManager.MoveNextLevelToCurrent();

            await StartLevelAsync( true );
         }
         else
         {
            await _cutsceneManager.PlayCutscene( _finalCutscene );
            SceneManager.LoadScene( "Menu" );
         }
      }

      private async void HandleCutsceneActionRequired( CutsceneEvent cutsceneEvent )
      {
         try
         {
            if(cutsceneEvent.EventType == CutsceneEvent.Type.SpawnLevel && _levelManager.CurrentLevelState == LevelManager.LevelState.ReadyToSpawn)
            {
               await _levelManager.SpawnCurrentLevel();
            }

            cutsceneEvent.Handled = true;
         }
         catch(Exception e)
         {
            Debug.LogError( e );
         }
      }

      private void OnDestroy()
      {
         _levelManager.OnCurrentLevelEnded.RemoveListener( HandleCurrentLevelEnded );
         _cutsceneManager.OnActionRequired.RemoveListener( HandleCutsceneActionRequired );
      }

      private void HandleCurrentLevelEnded() => EndLevelAsync().Forget();
   }
}