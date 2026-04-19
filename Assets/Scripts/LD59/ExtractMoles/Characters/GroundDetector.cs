using UnityEngine;

namespace LD59.ExtractMoles.Characters
{
   public class GroundDetector : MonoBehaviour
   {
      [SerializeField] private Vector3 _localOrigin = new(0, .2f, 0);
      [SerializeField] private float _distanceWithHitToBeGrounded = .05f;
      [SerializeField] private LayerMask _groundMask = ~0;

      public float GroundWorldY { get; private set; }

      public bool Grounded => transform.position.y - GroundWorldY <= _distanceWithHitToBeGrounded;

      private void Update()
      {
         if(Physics.Raycast( new Ray( transform.TransformPoint( _localOrigin ), Vector3.down ), out var hit, Mathf.Infinity, _groundMask, QueryTriggerInteraction.Ignore ))
         {
            GroundWorldY = hit.point.y;
         }
         else
         {
            GroundWorldY = transform.position.y - 2;
         }
      }

      private void OnDrawGizmos()
      {
         Gizmos.color = Color.blue;
         Gizmos.matrix = transform.localToWorldMatrix;
         Gizmos.DrawLine( _localOrigin, new Vector3( 0, -_distanceWithHitToBeGrounded, 0 ) );
      }
   }
}