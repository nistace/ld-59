using UnityEngine;

namespace LD59.ExtractMoles.Utilities
{
   [CreateAssetMenu]
   public class SpawnWithScaleConfigurationData : ScriptableObject
   {
      [SerializeField] private SpawnWithScale.Data _data = new();

      public SpawnWithScale.Data Data => _data;
   }
}