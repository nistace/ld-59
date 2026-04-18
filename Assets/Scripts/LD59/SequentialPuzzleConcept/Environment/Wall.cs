using UnityEngine;

namespace LD59.SequentialPuzzleConcept.Environment
{
   public class Wall : MonoBehaviour, ILevelObject
   {
      public bool IsObstacle => true;
   }
}