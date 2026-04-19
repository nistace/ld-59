using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cutscenes;
using LD59.ExtractMoles.PlayerControllers;
using LD59.Levels;
using System;
using UnityEngine;

namespace LD59
{
   public class GameController : MonoBehaviour
   {
      [SerializeField] private LevelManager _levelManager;
      [SerializeField] private CutsceneManager _cutsceneManager;

      private void Start()
      {
         _levelManager.OnCurrentLevelEnded.AddListener( HandleCurrentLevelEnded );
         _cutsceneManager.OnActionRequired.AddListener( HandleCutsceneActionRequired );
         CutsceneRequests.OnRequestReceived.AddListener( HandleCutsceneRequested );

         _levelManager.InstantiateFirstLevel();

         StartLevelAsync().Forget();
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

      private async UniTask StartLevelAsync()
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

         if(_levelManager.CurrentLevel.IntroScript)
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
            await _levelManager.DespawnCurrentLevel();
         }

         if(_levelManager.HasNextLevel())
         {
            _levelManager.MoveNextLevelToCurrent();

            await StartLevelAsync();
         }
         else
         {
            // TODO Return to menu
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