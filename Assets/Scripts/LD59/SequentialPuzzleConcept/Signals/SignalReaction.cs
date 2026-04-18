using Cysharp.Threading.Tasks;
using LD59.SequentialPuzzleConcept.ReactionTypes;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

namespace LD59.SequentialPuzzleConcept.Signals
{
   [Serializable]
   public class SignalReaction
   {
      [SerializeField] private SignalReactor _reactor;
      [SerializeField] private SignalSource _source;

      [FormerlySerializedAs( "_reactionSet" ), SerializeField]
      private ListOfReactionTypes _reactionTypes;

      public SignalReaction( SignalReactor reactor, SignalSource source, ListOfReactionTypes listOfReactionTypes )
      {
         _reactor = reactor;
         _source = source;
         _reactionTypes = listOfReactionTypes;
      }

      public UniTask Execute( LevelData level, CancellationToken cancellationToken ) => _reactionTypes.Execute( _reactor.LevelObject, level, cancellationToken );

      public override string ToString() => $"Reaction of {_reactor.ReactorName} to signal {_source.SignalName}";
   }
}