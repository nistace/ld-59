using LD59.ExtractMoles.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.Levels
{
   [RequireComponent( typeof(BoxCollider) )]
   public class RoomExit : MonoBehaviour
   {
      [SerializeField] private GameObject _playerBlocker;
      [SerializeField] private int _requiredMoles = 1;

      private int ExitedMoles { get; set; }

      public UnityEvent OnEveryoneLeftRoom { get; } = new();

      private void OnTriggerEnter( Collider other )
      {
         var character = other.GetComponentInParent<Character>();

         if(!character)
         {
            return;
         }

         if(ExitedMoles == _requiredMoles)
         {
            OnEveryoneLeftRoom.Invoke();
         }
         else
         {
            ExitedMoles++;
         }

         _playerBlocker.gameObject.SetActive( ExitedMoles < _requiredMoles );
      }
   }
}