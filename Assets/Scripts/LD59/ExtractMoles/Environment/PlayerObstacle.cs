using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   public class PlayerObstacle : MonoBehaviour
   {
      [SerializeField] private BoxCollider _boxCollider;

      private void OnDrawGizmos()
      {
         Gizmos.color = new Color( 1, 0, 0, .3f );
         Gizmos.matrix = transform.localToWorldMatrix;
         Gizmos.DrawCube( _boxCollider.center, _boxCollider.size );
      }
   }
}