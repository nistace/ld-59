using UnityEngine;

namespace LD59.SequentialPuzzleConcept.Grids
{
   public static class GridUtility
   {
      public static Vector2Int GridPosition( this Transform transform ) => new(Mathf.FloorToInt( transform.position.x ), Mathf.FloorToInt( transform.position.z ));
      public static Vector3 GridPositionToWorldPosition( Vector2Int gridPosition ) => new(gridPosition.x + .5f, 0, gridPosition.y + .5f);
   }
}