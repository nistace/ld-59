using UnityEngine;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class PlayerInfo : MonoBehaviour
   {
      [SerializeField] private PlayerConfig _playerConfig;
      public bool LockedByInteraction { get; set; }
      public PlayerConfig Config => _playerConfig;
   }
}