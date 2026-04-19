using UnityEngine;

namespace LD59.ExtractMoles.Utilities
{
   public static class ColorUtility
   {
      public static Color Alpha( this Color c, float a ) => new(c.r, c.g, c.b, a);
      public static Color MoveAlpha( this Color c, float deltaAlpha ) => new(c.r, c.g, c.b, Mathf.Clamp01( c.a + deltaAlpha ));
   }
}