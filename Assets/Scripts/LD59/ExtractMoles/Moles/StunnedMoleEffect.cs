using UnityEngine;

namespace LD59.ExtractMoles.Moles
{
   public class StunnedMoleEffect : MonoBehaviour
   {
      [SerializeField] private float _growSpeed;

      private bool TargetScale { get; set; }

      public void SetTargetScale( bool scale )
      {
         TargetScale = scale;
         gameObject.SetActive( true );
      }

      private void Update()
      {
         transform.localScale = Vector3.one * Mathf.MoveTowards( transform.localScale.x, TargetScale ? 1 : 0, Time.deltaTime * _growSpeed );

         if(Mathf.Approximately( transform.localScale.x, 0 ) && !TargetScale)
         {
            gameObject.SetActive( false );
         }
      }
   }
}