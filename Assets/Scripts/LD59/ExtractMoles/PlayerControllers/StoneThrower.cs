using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Cameras;
using LD59.ExtractMoles.Characters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class StoneThrower : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      [SerializeField] private InputActionReference _cursorActionReference;
      [SerializeField] private InputActionReference _triggerActionReference;
      [SerializeField] private InputActionReference _activateMode;
      [SerializeField] private float _radius;
      [SerializeField] private Transform _radiusVisual;
      [SerializeField] private Transform _stone;
      [SerializeField] private AnimationCurve _heightCurve;
      [SerializeField] private AnimationCurve _heightPerDistance;
      [SerializeField] private LayerMask _stoneHitMask = ~0;
      [SerializeField] private float _stoneSpeed;
      [SerializeField] private LayerMask _stoneSoundsLayerMask = ~0;
      [SerializeField] private Collider[] _nonAllocColliders = new Collider[ 16 ];

      private bool IsThrowing { get; set; }

      private void OnDisable()
      {
         if(_radiusVisual)
         {
            _radiusVisual.gameObject.SetActive( false );
         }
      }

      private void Update()
      {
         if(IsThrowing)
         {
            _radiusVisual.gameObject.SetActive( false );
            return;
         }

         if(_activateMode != null && !_activateMode.action.IsPressed())
         {
            _radiusVisual.gameObject.SetActive( false );
            return;
         }

         var ray = GameplayCamera.Camera.ScreenPointToRay( _cursorActionReference.action.ReadValue<Vector2>() );
         if(Physics.Raycast( ray, out var hit, _stoneHitMask ))
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

            progress += (Time.deltaTime * _stoneSpeed) / distance;

            await UniTask.NextFrame();
         }

         _stone.gameObject.SetActive( false );

         var hits = Physics.OverlapSphereNonAlloc( targetPoint, _radius, _nonAllocColliders, _stoneSoundsLayerMask );

         if(hits == _nonAllocColliders.Length)
         {
            Debug.LogWarning( "Are there enough colliders?" );
         }

         for(var hitIndex = 0; hitIndex < hits; hitIndex++)
         {
            _nonAllocColliders[ hitIndex ].gameObject.GetComponentInParent<IStoneSoundReactor>()?.ReactToStoneSound( targetPoint );
         }

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