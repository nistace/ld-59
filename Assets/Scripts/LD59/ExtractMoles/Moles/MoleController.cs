using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Signals;
using System;
using UnityEngine;

namespace LD59.ExtractMoles.Moles
{
   [RequireComponent( typeof(Character) )]
   public class MoleController : MonoBehaviour, IStoneSoundReactor, ISignalListener
   {
      [SerializeField] private Character _character;
      [SerializeField] private float _acceleration = 1;
      [SerializeField] private float _maxSpeed = 3;
      [SerializeField] private float _deceleration = 1;
      [SerializeField] private float _knockedOutTime = 3;
      [SerializeField] private float _knockedOutSpeed = -.5f;

      private float KnockedOutCooldown { get; set; }
      private bool IsKnockedOut => KnockedOutCooldown > 0;
      private bool HasToRun { get; set; }
      private float Speed { get; set; }

      private void OnEnable() => SignalSystem.Register( this );
      private void OnDisable() => SignalSystem.Unregister( this );

      public void ReactToStoneSound( Vector3 stonePosition )
      {
         if(HasToRun) return;
         if(IsKnockedOut) return;
         if(!Mathf.Approximately( Speed, 0 )) return;

         transform.forward = new Vector3( stonePosition.x, transform.position.y, stonePosition.z ) - transform.position;
      }

      private void Update()
      {
         if(IsKnockedOut)
         {
            KnockedOutCooldown -= Time.deltaTime;
         }

         Speed = Mathf.MoveTowards( Speed, HasToRun ? _maxSpeed : 0, (HasToRun ? _acceleration : _deceleration) * Time.deltaTime );

         if(Mathf.Approximately( Speed, 0 ))
         {
            return;
         }

         if(_character.IsAboutToHitSomething( Speed ))
         {
            HasToRun = false;
            Speed *= _knockedOutSpeed;
            KnockedOutCooldown = _knockedOutTime;
         }
         else
         {
            transform.position += transform.forward * (Speed * Time.deltaTime);
         }
      }

      public void ReactToSignal()
      {
         if(IsKnockedOut) return;

         HasToRun = !HasToRun;

         if(HasToRun)
         {
            RunAsync().Forget();
         }
      }

      private async UniTask RunAsync()
      {
         float speed = 0;

         while(HasToRun)
         {
            speed = Mathf.MoveTowards( speed, _maxSpeed, _acceleration * Time.deltaTime );

            await UniTask.NextFrame();
         }

         while(speed > 0)
         {
            speed = Mathf.MoveTowards( speed, 0, _deceleration * Time.deltaTime );

            await UniTask.NextFrame();
         }
      }
   }
}