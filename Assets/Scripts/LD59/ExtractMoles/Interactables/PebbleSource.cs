using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Moles;
using LD59.ExtractMoles.PlayerControllers;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Interactables
{
   public class PebbleSource : MonoBehaviour, IInteractable
   {
      [SerializeField] private Transform _interactionPoint;
      [SerializeField] private Transform _uiOverlayAnchorPoint;
      [SerializeField] private MeshFilter _meshFilter;
      [SerializeField] private Mesh _collapsedMesh;
      [SerializeField] private UnityEvent _onCompleted;

      public Vector3 InteractionPoint => _interactionPoint.position;
      public Vector3 UIOverlayAnchorPoint => _uiOverlayAnchorPoint.position;

      private void Start()
      {
         MoleController.OnAnyMoleKnockedOut.AddListener( HandleAnyMoleKnockedOut );
      }

      private void HandleAnyMoleKnockedOut()
      {
         _meshFilter.sharedMesh = _collapsedMesh;
         MoleController.OnAnyMoleKnockedOut.RemoveListener( HandleAnyMoleKnockedOut );
      }

      private void OnDestroy() => MoleController.OnAnyMoleKnockedOut.RemoveListener( HandleAnyMoleKnockedOut );

      public async UniTask Interact( Transform interactor )
      {
         PlayerInfo.PlayerInstance.ActiveActions |= PlayerInfo.PlayerActions.ThrowStones;
         _onCompleted.Invoke();

         await UniTask.NextFrame();
         Destroy( this );
      }
   }
}