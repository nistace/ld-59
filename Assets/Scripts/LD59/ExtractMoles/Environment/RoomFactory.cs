using LD59.Levels;
using UnityEditor;
using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   public class RoomFactory : MonoBehaviour
   {
#if UNITY_EDITOR
      [SerializeField] private int _width = 8;
      [SerializeField] private int _depth = 8;
      [SerializeField] private GameObject _wallPrefab;
      [SerializeField] private GameObject _floorPrefab;
      [SerializeField] private RoomExit _roomExitPrefab;

      [ContextMenu( "Build" )]
      private void Build()
      {
         var newRoom = new GameObject( $"Room_{_width}x{_depth}" ).transform;
         newRoom.SetParent( transform );
         var staticRoom = new GameObject( $"Static" ).transform;
         staticRoom.SetParent( newRoom );

         for(var x = 1; x < _width; x += 2)
         for(var z = 1; z < _depth; z += 2)
         {
            var floor = (GameObject)PrefabUtility.InstantiatePrefab( _floorPrefab, staticRoom );
            floor.transform.position = new Vector3( x, 0, z );
         }

         for(var x = 0; x < _width; x++)
         {
            var wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, staticRoom );
            wall.transform.position = new Vector3( x + .5f, 0, .5f );

            wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, staticRoom );
            wall.transform.position = new Vector3( x + .5f, 0, _depth - .5f );

            if(x != 0 && x != _width - 1) continue;

            for(var z = 1; z < _depth - 1; z++)
            {
               wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, staticRoom );
               wall.transform.position = new Vector3( x + .5f, 0, z + .5f );
            }
         }

         var floorCollider = new GameObject( "FloorCollider" ).AddComponent<BoxCollider>();
         floorCollider.center = new Vector3( _width * .5f, -.1f, _depth * .5f );
         floorCollider.size = new Vector3( _width, .2f, _depth );
         floorCollider.transform.SetParent( newRoom.transform );

         var centerOfTheRoom = new GameObject( "CenterOfTheRoom" ).transform;
         centerOfTheRoom.position = new Vector3( _width * .5f, 0, _depth * .5f );
         centerOfTheRoom.transform.SetParent( newRoom.transform );

         var nextLevelAnchor = new GameObject( "NextLevelAnchor" ).transform;
         nextLevelAnchor.position = new Vector3( _width * .5f, 0, _depth * .5f );
         nextLevelAnchor.transform.SetParent( newRoom.transform );

         var roomExit = ((GameObject)PrefabUtility.InstantiatePrefab( _roomExitPrefab, staticRoom )).GetComponent<RoomExit>();
         roomExit.gameObject.layer = LayerMask.NameToLayer( "RoomExit" );
         roomExit.transform.SetParent( newRoom.transform );
         roomExit.transform.position = new Vector3( _width + 1.5f, 0, .5f );
      }
#endif
   }
}