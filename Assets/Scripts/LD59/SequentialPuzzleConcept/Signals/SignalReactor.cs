using LD59.SequentialPuzzleConcept.ReactionTypes;
using System;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.Signals
{
   public class SignalReactor : MonoBehaviour
   {
      [SerializeField] private string _reactorName;
      [SerializeField] private SignalSource[] _sources;
      [SerializeField] private ListOfReactionTypes[] _reactionSets;

      public string ReactorName => _reactorName;
      public ILevelObject LevelObject { get; private set; }

      private void Start()
      {
         LevelObject = GetComponent<ILevelObject>();
      }

      public bool TryGetReactionSet( SignalSource source, out ListOfReactionTypes listOfReactionTypes )
      {
         var index = Array.IndexOf( _sources, source );

         if(index >= 0)
         {
            listOfReactionTypes = _reactionSets[ index ];
            return true;
         }

         listOfReactionTypes = default;
         return false;
      }
   }
}