using LD59.ExtractMoles.Interactables;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class InteractionController : MonoBehaviour
   {
      [SerializeField] private float _interactForwardOffset = .1f;
      [SerializeField] private float _interactionRadius = .8f;
      [SerializeField] private InputActionReference _interactionAction;
      [SerializeField] private Collider[] _interactableColliders;
      [SerializeField] private LayerMask _interactableLayerMask;

      private IInteractable _currentInteractable;

      public IInteractable CurrentInteractable => _currentInteractable;
      private UnityEvent<IInteractable> OnInteractableChanged { get; } = new();

      private void OnEnable()
      {
         _interactionAction.action.performed += HandleInteractionPerformed;
      }

      private void OnDisable()
      {
         _interactionAction.action.performed -= HandleInteractionPerformed;
      }

      private void HandleInteractionPerformed( InputAction.CallbackContext context )
      {
         _currentInteractable?.Interact();
      }

      private void Update()
      {
         RefreshTargetInteractable();
      }

      private void RefreshTargetInteractable()
      {
         var interactPosition = transform.position + transform.forward * _interactForwardOffset;

         var collisions = Physics.OverlapSphereNonAlloc( interactPosition, _interactionRadius, _interactableColliders, _interactableLayerMask );

         var newInteractable = _interactableColliders.Take( collisions )
            .Select( t => t.GetComponentInParent<IInteractable>() )
            .Where( t => t != null )
            .OrderBy( t => Vector3.SqrMagnitude( t.InteractionPoint - interactPosition ) )
            .FirstOrDefault();

         if(newInteractable == _currentInteractable) return;

         _currentInteractable = newInteractable;

         OnInteractableChanged.Invoke( _currentInteractable );
      }
   }
}