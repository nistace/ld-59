using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Cutscenes;
using LD59.ExtractMoles.PlayerControllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.Levels
{
   public class Level : MonoBehaviour
   {
      [SerializeField] private Transform _centerOfRoom;
      [SerializeField] private RoomExit _roomExit;
      [SerializeField] private Transform _nextLevelAnchor;
      [SerializeField] private LevelFancySpawnable[] _objectsToKeepWithNextLevel;
      [SerializeField] private CharacterSpawner[] _moleSpawners;
      [SerializeField] private CharacterSpawner _playerSpawner;
      [SerializeField] private bool _spawnBeforeIntroCutscene = true;
      [SerializeField] private CutsceneScript _introScript;
      [SerializeField] private PlayerInfo.PlayerActions _activeActionsAfterCutscene = (PlayerInfo.PlayerActions)~0;
      [SerializeField] private UnityEvent _levelStarted = new();

      public Vector3 NextLevelAnchorPosition => _nextLevelAnchor.position;
      public IReadOnlyList<LevelFancySpawnable> ObjectsToKeepWithNextLevel => _objectsToKeepWithNextLevel;
      public Transform CenterOfTheRoom => _centerOfRoom;
      public CutsceneScript IntroScript => _introScript;
      public bool SpawnBeforeIntroCutscene => _spawnBeforeIntroCutscene;

      public CharacterSpawner[] MoleSpawners => _moleSpawners;
      public CharacterSpawner PlayerSpawner => _playerSpawner;
      public PlayerInfo.PlayerActions ActiveActionsAfterCutscene => _activeActionsAfterCutscene;

      public UnityEvent OnLevelEnded => _roomExit.OnEveryoneLeftRoom;

      private void Reset()
      {
         _centerOfRoom = GetComponentsInChildren<Transform>().FirstOrDefault( t => t.name == "CenterOfRoom" );
         _roomExit = GetComponentInChildren<RoomExit>();
         _centerOfRoom = GetComponentsInChildren<Transform>().FirstOrDefault( t => t.name == "NextLevelAnchor" );
      }

      private void OnDrawGizmos()
      {
         if(_centerOfRoom)
         {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere( _centerOfRoom.position, 0.1f );
         }

         if(_nextLevelAnchor)
         {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere( _nextLevelAnchor.position, 0.1f );
         }

         if(_roomExit)
         {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube( _roomExit.transform.position, Vector3.one * .5f );
         }

         Gizmos.color = Color.magenta;
         foreach(var keptObject in _objectsToKeepWithNextLevel)
         {
            Gizmos.DrawCube( keptObject.transform.position + Vector3.up, Vector3.one * .5f );
         }

         foreach(var moleSpawner in _moleSpawners)
         {
            Gizmos.color = moleSpawner.GizmoColor;
            Gizmos.matrix = moleSpawner.transform.localToWorldMatrix;
            Gizmos.DrawCube( Vector3.zero, Vector3.one * .2f );
            Gizmos.DrawCube( Vector3.zero + Vector3.forward * .3f, Vector3.one * .1f );
         }

         if(_playerSpawner)
         {
            Gizmos.color = _playerSpawner.GizmoColor;
            Gizmos.matrix = _playerSpawner.transform.localToWorldMatrix;
            Gizmos.DrawCube( Vector3.zero, Vector3.one * .2f );
            Gizmos.DrawCube( Vector3.zero + Vector3.forward * .3f, Vector3.one * .1f );
         }
      }

      public void StartLevel() => _levelStarted.Invoke();
   }
}