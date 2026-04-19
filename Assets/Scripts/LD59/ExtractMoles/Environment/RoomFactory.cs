using UnityEditor;
using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   public class RoomFactory : MonoBehaviour
   {
#if UNITY_EDITOR
      [SerializeField] private Transform _parent;
      [SerializeField] private int _width = 8;
      [SerializeField] private int _depth = 8;
      [SerializeField] private GameObject _wallPrefab;
      [SerializeField] private GameObject _floorPrefab;

      [ContextMenu( "Build" )]
      private void Build()
      {
         for(var x = 1; x < _width; x += 2)
         for(var z = 1; z < _depth; z += 2)
         {
            var floor = (GameObject)PrefabUtility.InstantiatePrefab( _floorPrefab, _parent );
            floor.transform.localPosition = new Vector3( x, 0, z );
         }

         for(var x = 0; x < _width; x++)
         {
            var wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, _parent );
            wall.transform.localPosition = new Vector3( x + .5f, 0, .5f );

            wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, _parent );
            wall.transform.localPosition = new Vector3( x + .5f, 0, _depth - .5f );

            if(x != 0 && x != _width - 1) continue;

            for(var z = 1; z < _depth - 1; z++)
            {
               wall = (GameObject)PrefabUtility.InstantiatePrefab( _wallPrefab, _parent );
               wall.transform.localPosition = new Vector3( x + .5f, 0, z + .5f );
            }
         }

         var floorCollider = new GameObject( "FloorCollider" ).AddComponent<BoxCollider>();
         floorCollider.center = new Vector3( _width * .5f, -.1f, _depth * .5f );
         floorCollider.size = new Vector3( _width, .2f, _depth );
         floorCollider.transform.SetParent( _parent );
         floorCollider.transform.localPosition = Vector3.zero;
      }

      [ContextMenu( "Fix All Children" )]
      private void FixAllChildren()
      {
         for(var i = 0; i < _parent.childCount; ++i)
         {
            var child = _parent.GetChild( i );

            child.localPosition = new Vector3( Mathf.RoundToInt( child.localPosition.x * 2 ) * .5f,
               Mathf.RoundToInt( child.localPosition.y ),
               Mathf.RoundToInt( child.localPosition.z * 2 ) * .5f );
         }
      }
#endif
   }
}