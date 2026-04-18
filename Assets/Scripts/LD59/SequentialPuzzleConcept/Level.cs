using Cysharp.Threading.Tasks;
using LD59.SequentialPuzzleConcept.Signals;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept
{
   public class Level : MonoBehaviour
   {
      [SerializeField] private SignalSource _initialSignalSource;
      [SerializeField] private SignalReactor[] _orderedReactors;

      private LevelData _levelData;
      private LevelExecution _execution;

      public void Start()
      {
         _levelData = new LevelData( GetComponentsInChildren<ILevelObject>() );
      }

      [ContextMenu( "Play" )]
      public void Play()
      {
         SignalSource.OnSignalEmitted.AddListener( HandleSignalEmitted );

         _execution = new LevelExecution( _levelData );

         _initialSignalSource.Emit();

         _execution.Execute().Forget();
      }

      private void HandleSignalEmitted( SignalSource source )
      {
         foreach(var reactor in _orderedReactors)
         {
            if(reactor.TryGetReactionSet( source, out var reactionSet ))
            {
               _execution.Enqueue( new SignalReaction( reactor, source, reactionSet ) );
            }
         }
      }
   }
}