using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Interactables;
using LD59.ExtractMoles.Signals;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

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
      [SerializeField] private UnityEvent _onKnockedOut = new();
      [SerializeField] private UnityEvent _onNotKnockedOut = new();

      private float KnockedOutCooldown { get; set; }
      public bool IsKnockedOut => KnockedOutCooldown > 0;
      private bool HasToRun { get; set; }
      private float Speed { get; set; }
      public float SpeedNormalized => Speed / _maxSpeed;

      public static UnityEvent OnAnyMoleKnockedOut { get; } = new();

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
         if(transform.localScale.x < .9f) return;

         if(IsKnockedOut)
         {
            KnockedOutCooldown -= Time.deltaTime;

            if(!IsKnockedOut)
            {
               _onNotKnockedOut.Invoke();
            }
         }

         _character.UpdateVerticalPosition();

         Speed = Mathf.MoveTowards( Speed, HasToRun ? _maxSpeed : 0, (HasToRun && _character.Grounded ? _acceleration : _deceleration) * Time.deltaTime );

         if(Mathf.Approximately( Speed, 0 ))
         {
            return;
         }

         if(_character.IsAboutToHitSomething( Speed, out var colliders, out var hits ))
         {
            var speedRatio = Speed / _maxSpeed;

            foreach(var reactor in colliders.Take( hits ).SelectMany( t => t.GetComponentsInParent<IReactToMoleHeadBump>() ).Where( t => t.RequiredSpeedRatio <= speedRatio ))
            {
               reactor.React();
            }

            HasToRun = false;
            Speed *= _knockedOutSpeed;
            KnockedOutCooldown = _knockedOutTime;
            _onKnockedOut.Invoke();
            OnAnyMoleKnockedOut.Invoke();
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