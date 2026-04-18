using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace LD59.Levels
{
   public class LevelManager : MonoBehaviour
   {
      [SerializeField] private GameplayCamera _camera;
      [SerializeField] private Level[] _levels;
      [SerializeField] private int _firstLevelIndex;
      [SerializeField] private float _fancySpawnableSpawnDuration = .5f;
      [SerializeField] private float _spawnDuration = 3;
      [SerializeField] private AnimationCurve _spawnAnimationCurve;

      private Level CurrentLevel { get; set; }
      private int CurrentLevelIndex { get; set; }

      public UnityEvent OnCurrentLevelEnded { get; } = new();

      public async UniTask GoToNextLevel()
      {
         var nextLevelSpawnPosition = Vector3.zero;
         IReadOnlyList<LevelFancySpawnable> itemsToKeepBetweenLevels = Array.Empty<LevelFancySpawnable>();

         if(CurrentLevel)
         {
            nextLevelSpawnPosition = CurrentLevel.NextLevelAnchorPosition;
            itemsToKeepBetweenLevels = CurrentLevel.ObjectsToKeepWithNextLevel;
            await Despawn();
            CurrentLevelIndex++;
            CurrentLevel.OnLevelEnded.RemoveListener( OnCurrentLevelEnded.Invoke );
         }
         else
         {
            CurrentLevelIndex = _firstLevelIndex;
         }

         await Spawn( CurrentLevelIndex, nextLevelSpawnPosition, itemsToKeepBetweenLevels );
      }

      private async UniTask Despawn()
      {
         var spawnables = CurrentLevel.GetComponentsInChildren<LevelFancySpawnable>()
            .Except( CurrentLevel.ObjectsToKeepWithNextLevel )
            .OrderByDescending( t => t.Priority )
            .ThenBy( _ => Random.value )
            .Select( t => new SpawningItem( t ) )
            .ToArray();

         await AnimateSpawningAsync( spawnables, false );
      }

      private async UniTask Spawn( int levelIndex, Vector3 position, IReadOnlyList<LevelFancySpawnable> itemFromPreviousLevel )
      {
         CurrentLevelIndex = levelIndex;
         CurrentLevel = Instantiate( _levels[ CurrentLevelIndex ], position, Quaternion.identity );
         CurrentLevel.gameObject.SetActive( false );
         _camera.CenterOfTheRoom = CurrentLevel.CenterOfTheRoom;
         CurrentLevel.OnLevelEnded.AddListener( OnCurrentLevelEnded.Invoke );

         await UniTask.NextFrame();

         foreach(var item in itemFromPreviousLevel)
         {
            item.transform.SetParent( CurrentLevel.transform );
         }

         var spawnables = CurrentLevel.GetComponentsInChildren<LevelFancySpawnable>()
            .OrderBy( t => t.Priority )
            .ThenBy( _ => Random.value )
            .Select( t => new SpawningItem( t ) )
            .ToArray();

         foreach(var spawnable in spawnables)
         {
            spawnable.Spawnable.transform.localScale = Vector3.zero;
         }

         CurrentLevel.gameObject.SetActive( true );

         await AnimateSpawningAsync( spawnables, true );
      }

      private async UniTask AnimateSpawningAsync( SpawningItem[] _allSpawningItems, bool forward )
      {
         List<SpawningItem> spawningItems = new();

         var spawnRoutine = AnimateItemsProgressAsync( spawningItems, forward );

         var durationBetweenSpawnables = (_spawnDuration - _fancySpawnableSpawnDuration) / (_allSpawningItems.Length - 1);

         var timeElapsed = 0f;
         var nextSpawnIndex = 0;
         while(nextSpawnIndex < _allSpawningItems.Length)
         {
            timeElapsed += Time.deltaTime;

            while(timeElapsed > 0 && nextSpawnIndex < _allSpawningItems.Length)
            {
               spawningItems.Add( _allSpawningItems[ nextSpawnIndex ] );
               timeElapsed -= durationBetweenSpawnables;
               nextSpawnIndex++;
            }

            await UniTask.NextFrame();
            timeElapsed += Time.deltaTime;
         }

         await spawnRoutine;
      }

      private async UniTask AnimateItemsProgressAsync( List<SpawningItem> spawningItems, bool forward )
      {
         await UniTask.WaitWhile( () => spawningItems.Count == 0 );

         while(spawningItems.Count > 0)
         {
            foreach(var spawningItem in spawningItems)
            {
               spawningItem.Progress += Time.deltaTime / _fancySpawnableSpawnDuration;

               var animationTime = Mathf.Clamp01( forward ? spawningItem.Progress : 1 - spawningItem.Progress );

               spawningItem.Spawnable.transform.localScale = Vector3.one * _spawnAnimationCurve.Evaluate( animationTime );
            }

            while(spawningItems.Count > 0 && spawningItems[ 0 ].Progress > 1)
            {
               spawningItems.RemoveAt( 0 );
            }

            await UniTask.NextFrame();
         }
      }

      private class SpawningItem
      {
         public LevelFancySpawnable Spawnable { get; }
         public float Progress { get; set; }

         public SpawningItem( LevelFancySpawnable spawnable )
         {
            Spawnable = spawnable;
         }
      }
   }
}