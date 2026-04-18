using Cysharp.Threading.Tasks;
using LD59.SequentialPuzzleConcept.Signals;
using System.Threading;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.ReactionTypes
{
   [CreateAssetMenu]
   public class EmitSignalReactionType : ReactionType
   {
      public override UniTask Execute( ILevelObject reactor, LevelData level, CancellationToken cancellationToken )
      {
         if(reactor is not ISignalSourceHolder signalSourceHolder)
         {
            return UniTask.CompletedTask;
         }

         signalSourceHolder.EmitSignal();

         return UniTask.CompletedTask;
      }
   }
}