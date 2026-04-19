using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Interactables;
using System.Threading;
using UnityEngine;

namespace LD59.ExtractMoles.Mechanisms
{
   public class Elevator : MonoBehaviour
   {
      [SerializeField] private StateHolder _linkedStateHolder;
      [SerializeField] private State _upState;
      [SerializeField] private Transform _elevatorObject;
      [SerializeField] private Vector3 _downPosition = new(0, .02f, 0);
      [SerializeField] private Vector3 _upPosition = Vector3.up;
      [SerializeField] private float _speed = 2;
      [SerializeField] private ElevatorPlatformContainer _container;

      private CancellationTokenSource _cancellationTokenSource;

      private void Start()
      {
         if(_linkedStateHolder)
         {
            _elevatorObject.localPosition = _linkedStateHolder.CurrentState == _upState ? _upPosition : _downPosition;
            _linkedStateHolder.OnStateChanged.AddListener( HandleStateChanged );
         }
         else
         {
            _elevatorObject.localPosition = _downPosition;
         }
      }

      private void HandleStateChanged( State newState )
      {
         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = new CancellationTokenSource();

         Animate( _linkedStateHolder.Is( _upState ) ? _upPosition : _downPosition, _cancellationTokenSource.Token ).Forget();
      }

      private async UniTask Animate( Vector3 targetLocalPosition, CancellationToken token )
      {
         while(_elevatorObject.localPosition != targetLocalPosition)
         {
            var delta = Vector3.MoveTowards( _elevatorObject.localPosition, targetLocalPosition, _speed * Time.deltaTime ) - _elevatorObject.localPosition;
            _elevatorObject.localPosition += delta;

            foreach(var content in _container.Content)
            {
               content.position += delta;
            }

            await UniTask.NextFrame( token );
         }
      }

      private void OnDestroy()
      {
         _linkedStateHolder.OnStateChanged.RemoveListener( HandleStateChanged );

         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = null;
      }
   }
}