using Cysharp.Threading.Tasks;
using LD59.SequentialPuzzleConcept.Signals;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept
{
   [Serializable]
   public class LevelExecution
   {
      [SerializeField] private List<SignalReaction> _reactionQueue = new();
      [SerializeField] private LevelData _levelData;

      private CancellationTokenSource _cancellationTokenSource;

      public LevelExecution( LevelData levelData )
      {
         _levelData = levelData;
      }

      public async UniTask Execute()
      {
         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = new CancellationTokenSource();

         while(_reactionQueue.Count > 0)
         {
            var nextReaction = _reactionQueue[ 0 ];
            _reactionQueue.RemoveAt( 0 );

            Debug.Log( nextReaction );

            await nextReaction.Execute( _levelData, _cancellationTokenSource.Token );
         }
      }

      public void Enqueue( SignalReaction signalReaction ) => _reactionQueue.Add( signalReaction );
   }
}