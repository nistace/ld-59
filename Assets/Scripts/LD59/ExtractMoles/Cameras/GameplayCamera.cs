using UnityEngine;

namespace LD59.ExtractMoles.Cameras
{
   public class GameplayCamera : MonoBehaviour
   {
      [SerializeField] private Transform _centerOfTheRoom;
      [SerializeField] private Transform _followCam;
      [SerializeField] private float _smoothness = .2f;

      private Vector3 _currentVelocity;

      public Transform CenterOfTheRoom
      {
         get => _centerOfTheRoom;
         set => _centerOfTheRoom = value;
      }

      private void Update()
      {
         if(_centerOfTheRoom == null && _followCam == null) return;

         if(_centerOfTheRoom == null)
         {
            transform.position = Vector3.SmoothDamp( transform.position, _followCam.position, ref _currentVelocity, _smoothness );
         }
         else if(_followCam == null)
         {
            transform.position = Vector3.SmoothDamp( transform.position, _centerOfTheRoom.position, ref _currentVelocity, _smoothness );
         }
         else
         {
            transform.position = Vector3.SmoothDamp( transform.position,
               Vector3.Lerp( _followCam.position, _centerOfTheRoom ? _centerOfTheRoom.position : _followCam.position, .5f ),
               ref _currentVelocity,
               _smoothness );
         }
      }
   }
}