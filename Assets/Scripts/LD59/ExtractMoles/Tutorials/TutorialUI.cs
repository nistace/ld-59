using UnityEngine;

namespace LD59.ExtractMoles.Tutorials
{
   public class TutorialUI : MonoBehaviour
   {
      [SerializeField] private Camera _camera;
      [SerializeField] private Transform[] _tutorials;

      private TutorialTrigger CurrentTrigger { get; set; }

      private void Start()
      {
         TutorialTrigger.OnTriggered.AddListener( HandleTutorialTriggered );
         TutorialTrigger.OnCancelled.AddListener( HandleTutorialCancelled );
      }

      private void HandleTutorialCancelled( TutorialTrigger cancelled )
      {
         if(CurrentTrigger == cancelled)
         {
            _tutorials[ (int)CurrentTrigger.Type ].gameObject.SetActive( false );
         }
      }

      private void OnDestroy()
      {
         TutorialTrigger.OnTriggered.RemoveListener( HandleTutorialTriggered );
         TutorialTrigger.OnCancelled.RemoveListener( HandleTutorialCancelled );
      }

      private void HandleTutorialTriggered( TutorialTrigger trigger )
      {
         if(CurrentTrigger != null)
         {
            _tutorials[ (int)CurrentTrigger.Type ].gameObject.SetActive( false );
         }

         CurrentTrigger = trigger;

         _tutorials[ (int)CurrentTrigger.Type ].gameObject.SetActive( true );
      }

      private void Update()
      {
         if(!CurrentTrigger) return;
         if(!CurrentTrigger.Attach) return;

         _tutorials[ (int)CurrentTrigger.Type ].position = _camera.WorldToScreenPoint( CurrentTrigger.Attach.transform.position );
      }
   }
}