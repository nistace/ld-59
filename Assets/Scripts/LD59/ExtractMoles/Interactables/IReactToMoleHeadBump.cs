namespace LD59.ExtractMoles.Interactables
{
   public interface IReactToMoleHeadBump
   {
      float RequiredSpeedRatio { get; }

      void React();
   }
}