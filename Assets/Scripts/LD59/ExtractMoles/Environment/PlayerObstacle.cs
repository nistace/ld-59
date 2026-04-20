using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   public class PlayerObstacle : MonoBehaviour
   {
      [SerializeField] private BoxCollider _boxCollider;
      [SerializeField] private Color _gizmoColor = new(1, 0, 0, .3f);

      private void OnDrawGizmos()
      {
         Gizmos.color = _gizmoColor;
         Gizmos.matrix = transform.localToWorldMatrix;
         Gizmos.DrawCube( _boxCollider.center, _boxCollider.size );
      }
   }
}