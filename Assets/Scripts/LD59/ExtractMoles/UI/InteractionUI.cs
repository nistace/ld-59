using LD59.ExtractMoles.PlayerControllers;
using UnityEngine;

namespace LD59.ExtractMoles.UI
{
   public class InteractionUI : MonoBehaviour
   {
      [SerializeField] private Camera _camera;
      [SerializeField] private CanvasGroup _canvasGroup;
      [SerializeField] private float _fadeSpeed = 4;

      private void Start()
      {
         _canvasGroup.alpha = 0;
      }

      private void Update()
      {
         if(InteractionController.Instance == null || InteractionController.Instance.CurrentInteractable == null)
         {
            _canvasGroup.alpha = Mathf.MoveTowards( _canvasGroup.alpha, 0, Time.deltaTime * _fadeSpeed );
            return;
         }

         _canvasGroup.alpha = Mathf.MoveTowards( _canvasGroup.alpha, 1, Time.deltaTime * _fadeSpeed );
         transform.position = _camera.WorldToScreenPoint( InteractionController.Instance.CurrentInteractable.UIOverlayAnchorPoint );
      }
   }
}