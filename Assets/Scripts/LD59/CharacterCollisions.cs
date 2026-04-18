using System.Linq;
using UnityEngine;

namespace LD59
{
   public static class CharacterCollisions
   {
      private static readonly Collider[] Colliders = new Collider[ 5 ];

      public static bool IsAboutToHitSomething( this Transform transform, float radius, float speed, LayerMask _mask )
      {
         var hits = Physics.OverlapSphereNonAlloc( transform.position + Vector3.up * (radius + .1f) + transform.forward * (speed * Time.deltaTime), radius, Colliders, _mask );

         if((_mask.value & (1 << transform.gameObject.layer)) == 0)
         {
            return hits > 0;
         }

         return Colliders.Take( hits ).Any( hitCollider => !hitCollider.gameObject.transform.IsChildOf( transform ) );
      }
   }
}