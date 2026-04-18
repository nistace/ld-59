using LD59.SequentialPuzzleConcept.Grids;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept
{
   public interface ILevelObject
   {
      // ReSharper disable once InconsistentNaming
      Transform transform { get; }
      bool IsObstacle { get; }

      Vector2Int GridPosition => transform.GridPosition();
   }
}