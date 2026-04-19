using UnityEngine;
using UnityEngine.Events;

namespace LD59.ExtractMoles.Tutorials
{
   public class TutorialTrigger : MonoBehaviour
   {
      [SerializeField] private Transform _attach;
      [SerializeField] private TutorialType _type;

      public Transform Attach => _attach;
      public TutorialType Type => _type;

      public static UnityEvent<TutorialTrigger> OnTriggered { get; } = new();
      public static UnityEvent<TutorialTrigger> OnCancelled { get; } = new();

      public void Trigger() => OnTriggered.Invoke( this );
      public void Cancel() => OnCancelled.Invoke( this );
   }
}