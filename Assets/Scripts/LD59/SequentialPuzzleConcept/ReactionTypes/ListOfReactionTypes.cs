using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.ReactionTypes
{
   [Serializable]
   public class ListOfReactionTypes
   {
      [SerializeField] private ReactionType[] _reactionTypes;

      public async UniTask Execute( ILevelObject actor, LevelData level, CancellationToken cancellationToken )
      {
         foreach(var reactionType in _reactionTypes)
         {
            await reactionType.Execute( actor, level, cancellationToken );
         }
      }
   }
}