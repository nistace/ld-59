using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Characters;
using LD59.ExtractMoles.Interactables;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class InteractionController : MonoBehaviour, INotifiedOfCharacterDespawn, ILadderInteractor
   {
      public static InteractionController Instance { get; private set; }

      [SerializeField] private PlayerInfo _playerInfo;
      [SerializeField] private InputActionReference _interactionAction;
      [SerializeField] private Collider[] _interactableColliders;
      [SerializeField] private LayerMask _interactableLayerMask;

      private IInteractable _currentInteractable;
      public float ClimbSpeed => _playerInfo.Config.Movement.ClimbSpeed;
      public float MovementSpeed => _playerInfo.Config.Movement.MovementSpeed;

      public IInteractable CurrentInteractable => _currentInteractable;
      private UnityEvent<IInteractable> OnInteractableChanged { get; } = new();

      private void Awake()
      {
         Instance = this;
      }

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
         DoInteractionAsync().Forget();
      }

      private async UniTask DoInteractionAsync()
      {
         if(_playerInfo.LockedByInteraction) return;
         if(_currentInteractable == null) return;

         _playerInfo.LockedByInteraction = true;
         await _currentInteractable.Interact( transform );
         _playerInfo.LockedByInteraction = false;
      }

      private void Update()
      {
         RefreshTargetInteractable();
      }

      private void RefreshTargetInteractable()
      {
         if(!_playerInfo.Can( PlayerInfo.PlayerActions.Interact )) return;

         if(_playerInfo.LockedByInteraction)
         {
            if(_currentInteractable != null)
            {
               _currentInteractable = null;
               OnInteractableChanged.Invoke( _currentInteractable );
            }

            return;
         }

         var interactPosition = transform.position + transform.forward * _playerInfo.Config.Interactions.InteractForwardOffset;

         var collisions = Physics.OverlapSphereNonAlloc( interactPosition, _playerInfo.Config.Interactions.InteractionRadius, _interactableColliders, _interactableLayerMask );

         var newInteractable = _interactableColliders.Take( collisions )
            .Select( t => t.GetComponentInParent<IInteractable>() )
            .Where( t => t != null )
            .OrderBy( t => Vector3.SqrMagnitude( t.InteractionPoint - interactPosition ) )
            .FirstOrDefault();

         if(newInteractable == _currentInteractable) return;

         _currentInteractable = newInteractable;

         OnInteractableChanged.Invoke( _currentInteractable );
      }

      public void OnDespawn() => enabled = false;
   }
}