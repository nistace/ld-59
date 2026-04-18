using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.ReactionTypes
{
   public abstract class ReactionType : ScriptableObject
   {
      public abstract UniTask Execute( ILevelObject reactor, LevelData level, CancellationToken cancellationToken );
   }
}