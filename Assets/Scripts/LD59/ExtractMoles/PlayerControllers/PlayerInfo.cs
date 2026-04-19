using LD59.ExtractMoles.Characters;
using System;
using UnityEngine;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class PlayerInfo : MonoBehaviour, INotifiedOfCharacterDespawn
   {
      public static PlayerInfo PlayerInstance { get; private set; }

      [Flags]
      public enum PlayerActions
      {
         Move = 1 << 0,
         Whistle = 1 << 1,
         ThrowStones = 1 << 2,
         Interact = 1 << 3
      }

      [SerializeField] private PlayerConfig _playerConfig;
      public bool LockedByInteraction { get; set; }
      public PlayerConfig Config => _playerConfig;

      public PlayerActions ActiveActions { get; set; } = 0;

      public bool Can( PlayerActions actions ) => (ActiveActions & actions) == actions;
      public void OnDespawn() => ActiveActions = 0;

      private void Awake() => PlayerInstance = this;
   }
}