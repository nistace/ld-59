using UnityEngine.Events;

namespace LD59.ExtractMoles.Cutscenes
{
   public static class CutsceneRequests
   {
      public static UnityEvent<ICutsceneRequester> OnRequestReceived { get; } = new();

      public static void Request( ICutsceneRequester requester ) => OnRequestReceived.Invoke( requester );
   }
}