using LD59.ExtractMoles.Cutscenes;
using LD59.ExtractMoles.PlayerControllers;
using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Moles
{
   public class MoleKnockedCutscene : MonoBehaviour, ICutsceneRequester
   {
      [SerializeField] private CutsceneScript _cutsceneScript;
      [SerializeField] private UnityEvent _cutsceneEnded = new();

      private bool Triggered { get; set; }
      public CutsceneScript Script => _cutsceneScript;
      public bool RemoveAllPlayerActionsOnStart => true;
      public PlayerInfo.PlayerActions PlayerActionsAfterCutscene => PlayerInfo.PlayerActions.Whistle | PlayerInfo.PlayerActions.Move | PlayerInfo.PlayerActions.Interact;

      public void OnCutsceneEnded()
      {
         PlayerInfo.PlayerInstance.ActiveActions = PlayerActionsAfterCutscene;
         _cutsceneEnded.Invoke();
      }

      private void Start()
      {
         MoleController.OnAnyMoleKnockedOut.AddListener( HandleMoleKnockedOut );
      }

      private void OnDestroy()
      {
         MoleController.OnAnyMoleKnockedOut.RemoveListener( HandleMoleKnockedOut );
      }

      private void HandleMoleKnockedOut()
      {
         if(Triggered) return;

         MoleController.OnAnyMoleKnockedOut.RemoveListener( HandleMoleKnockedOut );
         Triggered = true;
         CutsceneRequests.Request( this );
      }
   }
}