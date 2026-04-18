using System.Linq;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept
{
   public class LevelData
   {
      private readonly ILevelObject[] _allLevelObjects;

      public LevelData( ILevelObject[] allLevelObjects )
      {
         _allLevelObjects = allLevelObjects;
      }

      public bool HasObstacle( Vector2Int gridPosition ) => _allLevelObjects.Any( t => t.GridPosition == gridPosition && t.IsObstacle );
   }
}