using System;
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

      public Vector3 NextLevelAnchorPosition => _nextLevelAnchor.position;
      public IReadOnlyList<LevelFancySpawnable> ObjectsToKeepWithNextLevel => _objectsToKeepWithNextLevel;
      public Transform CenterOfTheRoom => _centerOfRoom;

      public UnityEvent OnLevelEnded => _roomExit.OnEveryoneLeftRoom;

      private void Reset()
      {
         _centerOfRoom = GetComponentsInChildren<Transform>().FirstOrDefault( t => t.name == "CenterOfRoom" );
         _roomExit = GetComponentInChildren<RoomExit>();
         _centerOfRoom = GetComponentsInChildren<Transform>().FirstOrDefault( t => t.name == "NextLevelAnchor" );
      }

      private void OnDrawGizmos()
      {
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere( _centerOfRoom.position, 0.1f );
         Gizmos.color = Color.red;
         Gizmos.DrawSphere( _nextLevelAnchor.position, 0.1f );
         Gizmos.color = Color.yellow;
         Gizmos.DrawCube( _roomExit.transform.position, Vector3.one * .5f );
         Gizmos.color = Color.magenta;
         foreach(var keptObject in _objectsToKeepWithNextLevel)
         {
            Gizmos.DrawCube( keptObject.transform.position + Vector3.up, Vector3.one * .5f );
         }
      }
   }
}