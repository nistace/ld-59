using UnityEngine;

namespace LD59.ExtractMoles.Interactables
{
   public interface IInteractable
   {
      void Interact();
      Vector3 InteractionPoint { get; }
      Vector3 UIOverlayAnchorPoint { get; }
   }
}