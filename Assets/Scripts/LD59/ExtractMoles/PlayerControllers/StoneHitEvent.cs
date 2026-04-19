using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.PlayerControllers
{
   public class StoneHitEvent : MonoBehaviour
   {
      [SerializeField] private UnityEvent _event = new();

      private void Start() => StoneThrower.OnAnyStoneHit.AddListener( HandleSignalEmitted );
      private void OnDestroy() => StoneThrower.OnAnyStoneHit.RemoveListener( HandleSignalEmitted );

      private void HandleSignalEmitted() => _event.Invoke();
   }
}