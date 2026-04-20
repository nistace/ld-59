using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cameras;
using LD59.ExtractMoles.PlayerControllers;
using LD59.ExtractMoles.Utilities;
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

      public enum LevelState
      {
         Null = 0,
         ReadyToSpawn = 1,
         Spawned = 2,
         Despawned = 3
      }

      public Level CurrentLevel { get; private set; }
      public LevelState CurrentLevelState { get; private set; }
      private Level NextLevel { get; set; }
      private List<LevelFancySpawnable> ItemsFromPreviousLevel { get; } = new();
      private int CurrentLevelIndex { get; set; }
      private int NextLevelIndex { get; set; }

      public UnityEvent OnCurrentLevelEnded { get; } = new();

      public void InstantiateFirstLevel()
      {
         CurrentLevelIndex = _firstLevelIndex;
         CurrentLevel = Instantiate( _levels[ CurrentLevelIndex ], Vector3.zero, Quaternion.identity );
         CurrentLevel.gameObject.SetActive( false );
         CurrentLevelState = LevelState.ReadyToSpawn;
      }

      public bool HasNextLevel() => CurrentLevelIndex < _levels.Length - 1;

      public Level InstantiateNextLevel()
      {
         NextLevelIndex = CurrentLevelIndex + 1;
         NextLevel = Instantiate( _levels[ NextLevelIndex ], CurrentLevel.NextLevelAnchorPosition, Quaternion.identity );
         NextLevel.gameObject.SetActive( false );

         return NextLevel;
      }

      public Level InstantiateCurrentLevelAsNext()
      {
         NextLevelIndex = CurrentLevelIndex;
         NextLevel = Instantiate( _levels[ NextLevelIndex ], CurrentLevel.transform.position, Quaternion.identity );
         NextLevel.gameObject.SetActive( false );

         return NextLevel;
      }

      public void SaveItemsToKeepBeforeNextLevel()
      {
         ItemsFromPreviousLevel.Clear();
         ItemsFromPreviousLevel.AddRange( CurrentLevel.ObjectsToKeepWithNextLevel );
      }

      public async UniTask DespawnCurrentLevel()
      {
         foreach(var itemToNotify in ItemsFromPreviousLevel.SelectMany( t => t.GetComponentsInChildren<ILevelKeptItem>() ))
         {
            itemToNotify.NotifyItemKept();
         }

         foreach(var itemFromPreviousLevel in ItemsFromPreviousLevel)
         {
            itemFromPreviousLevel.transform.SetParent( null );
         }

         CurrentLevel.OnLevelEnded.RemoveListener( OnCurrentLevelEnded.Invoke );

         var spawnables = CurrentLevel.GetComponentsInChildren<LevelFancySpawnable>()
            .Except( CurrentLevel.ObjectsToKeepWithNextLevel )
            .OrderByDescending( t => t.Priority )
            .ThenBy( _ => Random.value )
            .ToArray();

         await AnimateSpawningAsync( spawnables, _despawnData.Data );

         Destroy( CurrentLevel.gameObject );
         CurrentLevelState = LevelState.Despawned;
      }

      public void MoveNextLevelToCurrent()
      {
         CurrentLevelIndex = NextLevelIndex;
         CurrentLevel = NextLevel;
         NextLevel = null;
         CurrentLevelState = LevelState.ReadyToSpawn;
      }

      public async UniTask SpawnCurrentLevel()
      {
         _camera.CenterOfTheRoom = CurrentLevel.CenterOfTheRoom;
         CurrentLevel.OnLevelEnded.AddListener( OnCurrentLevelEnded.Invoke );

         await UniTask.NextFrame();

         var spawnables = CurrentLevel.GetComponentsInChildren<LevelFancySpawnable>().OrderBy( t => t.Priority ).ThenBy( _ => Random.value ).ToArray();

         foreach(var itemFromPreviousLevel in ItemsFromPreviousLevel)
         {
            itemFromPreviousLevel.transform.SetParent( CurrentLevel.transform );
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
         player.GetComponent<PlayerInfo>().ActiveActions = 0;
         _camera.FollowCam = player.transform;

         CurrentLevelState = LevelState.Spawned;
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