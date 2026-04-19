using LD59.ExtractMoles.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   [RequireComponent( typeof(Character) )]
   public class MovementController : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      [SerializeField] private PlayerInfo _playerInfo;
      [SerializeField] private Character _character;
      [SerializeField] private InputActionReference _movementActionReference;

      private Vector2 _movement;
      private Vector2 _movementSmoothness;
      private bool _keepMovementGoing;

      private PlayerConfig.MovementData Config => _playerInfo.Config.Movement;

      private void Update()
      {
         if(_playerInfo.LockedByInteraction) return;

         if(_keepMovementGoing)
         {
            transform.position += new Vector3( _movement.x, 0, _movement.y ) * (_movement.magnitude * Config.MovementSpeed * Time.deltaTime);
            return;
         }

         _character.UpdateGravity();

         _movement = Vector2.SmoothDamp( _movement, _movementActionReference.action.ReadValue<Vector2>(), ref _movementSmoothness, Config.SmoothForward );

         if(_movement != Vector2.zero)
         {
            transform.forward = new Vector3( _movement.x, 0, _movement.y );
         }

         if(!_character.IsAboutToHitSomething( _movement.magnitude * Config.MovementSpeed ))
         {
            transform.position += new Vector3( _movement.x, 0, _movement.y ) * (_movement.magnitude * Config.MovementSpeed * Time.deltaTime);
         }
      }

      public void OnDespawn() => _keepMovementGoing = true;
   }
}