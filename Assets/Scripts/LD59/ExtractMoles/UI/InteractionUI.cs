using LD59.ExtractMoles.PlayerControllers;
using UnityEngine;

namespace LD59.ExtractMoles.UI
{
   public class InteractionUI : MonoBehaviour
   {
      [SerializeField] private Camera _camera;
      [SerializeField] private InteractionController _interactionController;
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private float _fadeSpeed = 4;

      private void Start()
      {
         _canvasGroup.alpha = 0;
      }

      private void Update()
      {
         _canvasGroup.alpha = Mathf.MoveTowards( _canvasGroup.alpha, _interactionController.CurrentInteractable == null ? 0 : 1, Time.deltaTime * _fadeSpeed );

         if(_interactionController.CurrentInteractable != null)
         {
            transform.position = _camera.WorldToScreenPoint( _interactionController.CurrentInteractable.UIOverlayAnchorPoint );
         }
      }
   }
}