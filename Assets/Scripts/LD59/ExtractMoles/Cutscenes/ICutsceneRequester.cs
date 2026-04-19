using LD59.ExtractMoles.PlayerControllers;

namespace LD59.ExtractMoles.Cutscenes
{
   public interface ICutsceneRequester
   {
      CutsceneScript Script { get; }
      bool RemoveAllPlayerActionsOnStart { get; }
      PlayerInfo.PlayerActions PlayerActionsAfterCutscene { get; }

      void OnCutsceneEnded();
   }
}