using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cameras;
using LD59.ExtractMoles.Characters;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class StoneThrower : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      [SerializeField] private PlayerInfo _playerInfo;
      [SerializeField] private InputActionReference _cursorActionReference;
      [SerializeField] private InputActionReference _triggerActionReference;
      [SerializeField] private InputActionReference _activateMode;
      [SerializeField] private Transform _radiusVisual;
      [SerializeField] private Transform _stone;
      [SerializeField] private AnimationCurve _heightCurve;
      [SerializeField] private AnimationCurve _heightPerDistance;
      [SerializeField] private Collider[] _nonAllocColliders = new Collider[ 16 ];
      [SerializeField] private UnityEvent _stoneThrown = new();
      [SerializeField] private UnityEvent _stoneHit = new();

      private PlayerConfig.StoneThrowingData Config => _playerInfo.Config.StoneThrowing;
      private bool IsThrowing { get; set; }

      public static UnityEvent OnAnyStoneHit { get; } = new();

      private void OnDisable()
      {
         if(_radiusVisual)
         {
            _radiusVisual.gameObject.SetActive( false );
         }
      }

      private bool IsInteractionAllowed()
      {
         if(_playerInfo.LockedByInteraction) return false;
         if(!_playerInfo.Can( PlayerInfo.PlayerActions.ThrowStones )) return false;
         if(IsThrowing) return false;
         if(_activateMode != null && !_activateMode.action.IsPressed()) return false;

         return true;
      }

      private void Update()
      {
         if(!IsInteractionAllowed())
         {
            _radiusVisual.gameObject.SetActive( false );
            return;
         }

         var ray = GameplayCamera.Camera.ScreenPointToRay( _cursorActionReference.action.ReadValue<Vector2>() );
         if(Physics.Raycast( ray, out var hit, 50, Config.StoneHitMask ))
         {
            _radiusVisual.position = hit.point;
            _radiusVisual.gameObject.SetActive( true );

            if(_triggerActionReference.action.WasPerformedThisFrame())
            {
               ThrowStone( hit.point ).Forget();
            }
         }
         else
         {
            _radiusVisual.gameObject.SetActive( false );
         }
      }

      private async UniTask ThrowStone( Vector3 targetPoint )
      {
         _stoneThrown.Invoke();

         IsThrowing = true;
         var origin = transform.position;
         var distance = Vector2.Distance( new Vector2( origin.x, origin.z ), new Vector2( targetPoint.x, targetPoint.z ) );
         var maxHeight = _heightPerDistance.Evaluate( distance );

         _stone.position = origin;
         _stone.gameObject.SetActive( true );

         var progress = 0f;
         while(progress < 1)
         {
            _stone.position = Vector3.Lerp( origin, targetPoint, progress ) + Vector3.up * (_heightCurve.Evaluate( progress ) * maxHeight);

            progress += (Time.deltaTime * Config.StoneSpeed) / distance;

            await UniTask.NextFrame();
         }

         _stone.gameObject.SetActive( false );

         var hits = Physics.OverlapSphereNonAlloc( targetPoint, Config.StoneNoiseRadius, _nonAllocColliders, Config.StoneSoundsLayerMask );

         if(hits == _nonAllocColliders.Length)
         {
            Debug.LogWarning( "Are there enough colliders?" );
         }

         for(var hitIndex = 0; hitIndex < hits; hitIndex++)
         {
            _nonAllocColliders[ hitIndex ].gameObject.GetComponentInParent<IStoneSoundReactor>()?.ReactToStoneSound( targetPoint );
         }

         _stoneHit.Invoke();
         OnAnyStoneHit.Invoke();

         IsThrowing = false;
      }

      public void OnDespawn()
      {
         enabled = false;
         _stone.gameObject.SetActive( false );
         _radiusVisual.gameObject.SetActive( false );
      }
   }
}