using LD59.ExtractMoles.Characters;
using UnityEngine;

namespace LD59.ExtractMoles.Mechanisms
{
   public class Grinder : MonoBehaviour
   {
      private void OnTriggerEnter( Collider other )
      {
         var character = other.GetComponentInParent<Character>();
         if(character)
         {
            CharacterEvents.OnCharacterDied.Invoke();
         }
      }
   }
}