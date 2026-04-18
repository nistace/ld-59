using Cysharp.Threading.Tasks;
using LD59.SequentialPuzzleConcept.Grids;
using System.Threading;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.ReactionTypes
{
   [CreateAssetMenu]
   public class MoveReactionType : ReactionType
   {
      [SerializeField] private Vector2Int _direction;

      public override async UniTask Execute( ILevelObject reactor, LevelData level, CancellationToken cancellationToken )
      {
         if(reactor is not IMoveableActor moveableReactor)
         {
            return;
         }

         while(!level.HasObstacle( reactor.GridPosition + _direction ))
         {
            var targetGridPosition = reactor.GridPosition + _direction;
            var targetWorldPosition = GridUtility.GridPositionToWorldPosition( targetGridPosition );

            while(reactor.transform.position != targetWorldPosition)
            {
               moveableReactor.MoveTowards( targetWorldPosition );
               await UniTask.NextFrame( cancellationToken: cancellationToken );
            }
         }
      }
   }
}