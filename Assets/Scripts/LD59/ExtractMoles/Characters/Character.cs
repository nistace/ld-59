using System.Linq;
using UnityEngine;

namespace LD59.ExtractMoles.Characters
{
   [RequireComponent( typeof(Rigidbody) )]
   public class Character : MonoBehaviour
   {
      private static readonly Collider[] Colliders = new Collider[ 5 ];

      [SerializeField] private CapsuleCollider _collider;
      [SerializeField] private LayerMask _collisionMask = ~1;

      public bool IsAboutToHitSomething( float speed )
      {
         var hits = Physics.OverlapSphereNonAlloc( transform.position + Vector3.up * (_collider.height * .5f) + transform.forward * (speed * Time.deltaTime),
            _collider.radius,
            Colliders,
            _collisionMask );

         if((_collisionMask.value & (1 << transform.gameObject.layer)) == 0)
         {
            return hits > 0;
         }

         return Colliders.Take( hits ).Any( hitCollider => !hitCollider.gameObject.transform.IsChildOf( transform ) );
      }
   }
}