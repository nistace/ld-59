using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Splines;
using Vector3 = UnityEngine.Vector3;

namespace LD59.ExtractMoles.Interactables
{
   public class Ladder : MonoBehaviour, IInteractable
   {
      [SerializeField] private Transform _interactionPoint;
      [SerializeField] private Transform _uiOverlayAnchorPoint;
      [SerializeField] private SplineContainer _splineContainer;

      public Vector3 InteractionPoint => _interactionPoint.position;
      public Vector3 UIOverlayAnchorPoint => _uiOverlayAnchorPoint.position;

      public async UniTask Interact( Transform interactor )
      {
         if(!interactor.TryGetComponent<ILadderInteractor>( out var ladderInteractor ))
         {
            return;
         }

         var bottomPoint = transform.TransformPoint( _splineContainer.Spline.EvaluatePosition( 0 ) );
         var upPoint = transform.TransformPoint( _splineContainer.Spline.EvaluatePosition( 1 ) );

         var start = bottomPoint;
         var startRatio = 0f;
         var end = upPoint;
         var endRatio = 1f;
         var splineProgressSpeed = _splineContainer.Spline.GetLength() / ladderInteractor.ClimbSpeed;

         if(Mathf.Abs( interactor.position.y - upPoint.y ) < Mathf.Abs( interactor.position.y - bottomPoint.y ))
         {
            start = upPoint;
            startRatio = 1;
            end = bottomPoint;
            endRatio = 0;
         }

         while(interactor.position != start)
         {
            interactor.forward = new Vector3( start.x - interactor.position.x, 0, start.z - interactor.position.z );
            interactor.position = Vector3.MoveTowards( interactor.position, start, Time.deltaTime * ladderInteractor.MovementSpeed );
            await UniTask.NextFrame();
         }

         interactor.forward = transform.forward;

         for(var progress = startRatio; !Mathf.Approximately( progress, endRatio ); progress = Mathf.MoveTowards( progress, endRatio, splineProgressSpeed * Time.deltaTime ))
         {
            interactor.position = transform.TransformPoint( _splineContainer.Spline.EvaluatePosition( progress ) );
            await UniTask.NextFrame();
         }

         interactor.position = end;
      }
   }
}