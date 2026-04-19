using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class PlayerMovedEvent : MonoBehaviour
   {
      [SerializeField] private UnityEvent _event = new();

      private Vector3 _location;
      private bool _savedLocation;

      private void Update()
      {
         if(!PlayerInfo.PlayerInstance) return;
         if(!PlayerInfo.PlayerInstance.Can( PlayerInfo.PlayerActions.Move )) return;
         if(!_savedLocation)
         {
            _location = PlayerInfo.PlayerInstance.transform.position;
            _savedLocation = true;
            return;
         }

         if(Vector3.SqrMagnitude( PlayerInfo.PlayerInstance.transform.position - _location ) < 1)
         {
            return;
         }

         _event.Invoke();
         Destroy( this );
      }
   }
}