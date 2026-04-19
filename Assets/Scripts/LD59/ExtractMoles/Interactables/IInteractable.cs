using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LD59.ExtractMoles.Interactables
{
   public interface IInteractable
   {
      UniTask Interact( Transform interactor );
      Vector3 InteractionPoint { get; }
      Vector3 UIOverlayAnchorPoint { get; }
   }
}