using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cameras;
using LD59.ExtractMoles.Utilities;
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
      [SerializeField] private SpawnWithScaleConfigurationData _spawnData;
      [SerializeField] private SpawnWithScaleConfigurationData _despawnData;
      [SerializeField] private float _spawnDuration = 3;

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

            foreach(var itemFromPreviousLevel in itemsToKeepBetweenLevels)
            {
               itemFromPreviousLevel.transform.SetParent( null );
            }

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
            .ToArray();

         await AnimateSpawningAsync( spawnables, _despawnData.Data );

         Destroy( CurrentLevel.gameObject );
      }

      private async UniTask Spawn( int levelIndex, Vector3 position, IReadOnlyList<LevelFancySpawnable> itemFromPreviousLevel )
      {
         CurrentLevelIndex = levelIndex;
         CurrentLevel = Instantiate( _levels[ CurrentLevelIndex ], position, Quaternion.identity );
         CurrentLevel.gameObject.SetActive( false );
         _camera.CenterOfTheRoom = CurrentLevel.CenterOfTheRoom;
         CurrentLevel.OnLevelEnded.AddListener( OnCurrentLevelEnded.Invoke );

         await UniTask.NextFrame();

         var spawnables = CurrentLevel.GetComponentsInChildren<LevelFancySpawnable>().OrderBy( t => t.Priority ).ThenBy( _ => Random.value ).ToArray();

         foreach(var item in itemFromPreviousLevel)
         {
            item.transform.SetParent( CurrentLevel.transform );
         }

         foreach(var spawnable in spawnables)
         {
            spawnable.transform.localScale = Vector3.zero;
         }

         CurrentLevel.gameObject.SetActive( true );

         await AnimateSpawningAsync( spawnables, _spawnData.Data );

         foreach(var moleSpawner in CurrentLevel.MoleSpawners)
         {
            await moleSpawner.SpawnAsync();
         }

         var player = await CurrentLevel.PlayerSpawner.SpawnAsync();
         _camera.FollowCam = player.transform;
      }

      private async UniTask AnimateSpawningAsync( LevelFancySpawnable[] _allSpawningItems, SpawnWithScale.Data spawnData )
      {
         List<UniTask> spawnTasks = new();

         var durationBetweenSpawnables = (_spawnDuration - spawnData.Speed) / (_allSpawningItems.Length - 1);

         var timeElapsed = 0f;
         var nextSpawnIndex = 0;
         while(nextSpawnIndex < _allSpawningItems.Length)
         {
            timeElapsed += Time.deltaTime;

            while(timeElapsed > 0 && nextSpawnIndex < _allSpawningItems.Length)
            {
               spawnTasks.Add( SpawnWithScale.Play( _allSpawningItems[ nextSpawnIndex ].transform, spawnData ) );

               timeElapsed -= durationBetweenSpawnables;
               nextSpawnIndex++;
            }

            await UniTask.NextFrame();
            timeElapsed += Time.deltaTime;
         }

         await UniTask.WhenAll( spawnTasks );
      }
   }
}