using UnityEngine;

namespace LD59.Levels
{
   public class LevelFancySpawnable : MonoBehaviour
   {
      [SerializeField] private int _priority;

      public int Priority => _priority;
   }
}