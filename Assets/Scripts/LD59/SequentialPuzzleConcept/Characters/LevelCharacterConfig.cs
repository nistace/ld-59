using UnityEngine;

namespace LD59.SequentialPuzzleConcept.Characters
{
   [CreateAssetMenu]
   public class LevelCharacterConfig : ScriptableObject
   {
      [SerializeField] private float _movementSpeed = 2;

      public float MovementSpeed => _movementSpeed;
   }
}