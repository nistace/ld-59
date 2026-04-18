using LD59.ExtractMoles.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   [RequireComponent( typeof(Character) )]
   public class MovementController : MonoBehaviour
   {
      [SerializeField] private Character _character;
      [SerializeField] private InputActionReference _movementActionReference;
      [SerializeField] private float _speed = 2;
      [SerializeField] private float _smoothDirection = .05f;

      private Vector2 _movement;
      private Vector2 _movementSmoothness;

      private void Update()
      {
         _movement = Vector2.SmoothDamp( _movement, _movementActionReference.action.ReadValue<Vector2>(), ref _movementSmoothness, _smoothDirection );

         if(_movement == Vector2.zero) return;

         transform.forward = new Vector3( _movement.x, 0, _movement.y );

         if(_character.IsAboutToHitSomething( _speed ))
         {
            return;
         }

         transform.position += new Vector3( _movement.x * _speed * Time.deltaTime, 0, _movement.y * _speed * Time.deltaTime );
      }
   }
}