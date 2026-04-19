using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Interactables;
using LD59.Levels;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Mechanisms
{
   public class Door : MonoBehaviour, ILevelKeptItem
   {
      [SerializeField] private StateHolder _linkedStateHolder;
      [SerializeField] private State _openState;
      [SerializeField] private Transform _doorObject;
      [SerializeField] private Vector3 _openLocalPosition = new(0, -1, 0);
      [SerializeField] private Vector3 _closeLocalPosition = Vector3.zero;
      [SerializeField] private float _speed = 4;
      [SerializeField] private UnityEvent _movement = new();

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
         if(_doorObject.localPosition != targetLocalPosition)
         {
            _movement.Invoke();
         }

         while(_doorObject.localPosition != targetLocalPosition)
         {
            _doorObject.localPosition = Vector3.MoveTowards( _doorObject.localPosition, targetLocalPosition, _speed * Time.deltaTime );

            await UniTask.NextFrame( token );
         }
      }

      private void OnDestroy()
      {
         if(_linkedStateHolder)
         {
            _linkedStateHolder.OnStateChanged.RemoveListener( HandleStateChanged );
         }

         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = null;
      }

      public void NotifyItemKept()
      {
         _cancellationTokenSource?.Cancel();
         _cancellationTokenSource?.Dispose();
         _cancellationTokenSource = new CancellationTokenSource();
         Animate( _closeLocalPosition, _cancellationTokenSource.Token ).Forget();
      }
   }
}