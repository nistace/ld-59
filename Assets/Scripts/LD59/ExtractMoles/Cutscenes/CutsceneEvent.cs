namespace LD59.ExtractMoles.Cutscenes
{
   public class CutsceneEvent
   {
      public enum Type
      {
         None = 0,
         SpawnLevel = 1
      }

      public Type EventType { get; }
      public bool Handled { get; set; }

      public CutsceneEvent( Type eventType)
      {
         EventType = eventType;
      }
   }
}