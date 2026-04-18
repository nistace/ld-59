using Vector3 = UnityEngine.Vector3;

namespace LD59.SequentialPuzzleConcept.ReactionTypes
{
   public interface IMoveableActor : ILevelObject
   {
      void MoveTowards( Vector3 worldPosition );
   }
}