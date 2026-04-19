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

      public Vector2 Movement { get; private set; }
      private Vector2 _movementSmoothness;
      private bool _keepMovementGoing;

      private PlayerConfig.MovementData Config => _playerInfo.Config.Movement;

      private void Update()
      {
         if(_keepMovementGoing)
         {
            transform.position += new Vector3( Movement.x, 0, Movement.y ) * (Movement.magnitude * Config.MovementSpeed * Time.deltaTime);
            return;
         }

         if(_playerInfo.LockedByInteraction) return;

         if(!_playerInfo.Can( PlayerInfo.PlayerActions.Move )) return;

         _character.UpdateVerticalPosition();

         Movement = Vector2.SmoothDamp( Movement, _movementActionReference.action.ReadValue<Vector2>(), ref _movementSmoothness, Config.SmoothForward );

         if(Movement != Vector2.zero)
         {
            transform.forward = new Vector3( Movement.x, 0, Movement.y );
         }

         if(!_character.IsAboutToHitSomething( Movement.magnitude * Config.MovementSpeed ))
         {
            transform.position += new Vector3( Movement.x, 0, Movement.y ) * (Movement.magnitude * Config.MovementSpeed * Time.deltaTime);
         }
      }

      public void OnDespawn() => _keepMovementGoing = true;
   }
}