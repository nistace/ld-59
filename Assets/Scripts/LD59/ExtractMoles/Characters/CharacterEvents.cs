using UnityEngine.Events;

namespace LD59.ExtractMoles.Characters
{
   public static class CharacterEvents
   {

      public static UnityEvent OnCharacterDied { get; } = new();

   }
}