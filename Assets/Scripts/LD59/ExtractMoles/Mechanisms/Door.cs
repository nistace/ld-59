using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Interactables;
using System.Threading;
using UnityEngine;

namespace LD59.ExtractMoles.Mechanisms
{
   public class Door : MonoBehaviour
   {
      [SerializeField] private StateHolder _linkedStateHolder;
      [SerializeField] private State _openState;
      [SerializeField] private Transform _doorObject;
      [SerializeField] private Vector3 _openLocalPosition = new(0, -1, 0);
      [SerializeField] private Vector3 _closeLocalPosition = Vector3.zero;
      [SerializeField] private float _speed = 4;

      private CancellationTokenSource _cancellationTokenSource;

      private void Start()
      {
         if(_linkedStateHolder)
         {
            _doorObject.localPosition = _linkedStateHolder.CurrentState == _openState ? _openLocalPosition : _closeLocalPosition;
            _linkedStateHolder.OnStateChanged.AddListener( HandleStateChanged );
         }
         else
         {
            _doorObject.localPosition = _closeLocalPosition;
         }
      }

      private void HandleStateChanged( State newState )
      {
         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = new CancellationTokenSource();

         Animate( _linkedStateHolder.Is( _openState ) ? _openLocalPosition : _closeLocalPosition, _cancellationTokenSource.Token ).Forget();
      }

      private async UniTask Animate( Vector3 targetLocalPosition, CancellationToken token )
      {
         while(_doorObject.localPosition != targetLocalPosition)
         {
            _doorObject.localPosition = Vector3.MoveTowards( _doorObject.localPosition, targetLocalPosition, _speed * Time.deltaTime );

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