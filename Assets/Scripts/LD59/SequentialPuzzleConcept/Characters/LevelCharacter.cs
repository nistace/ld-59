using LD59.SequentialPuzzleConcept.ReactionTypes;
using LD59.SequentialPuzzleConcept.Signals;
using UnityEngine;

namespace LD59.SequentialPuzzleConcept.Characters
{
   [RequireComponent( typeof(SignalSource) )]
   [RequireComponent( typeof(SignalReactor) )]
   public class LevelCharacter : MonoBehaviour, IMoveableActor, ISignalSourceHolder
   {
      [SerializeField] private SignalSource _signalSource;
      [SerializeField] private LevelCharacterConfig _config;

      public bool IsObstacle => true;

      public void MoveTowards( Vector3 worldPosition )
      {
         transform.position = Vector3.MoveTowards( transform.position, worldPosition, Time.deltaTime * _config.MovementSpeed );
      }

      public void EmitSignal() => _signalSource.Emit();
   }
}