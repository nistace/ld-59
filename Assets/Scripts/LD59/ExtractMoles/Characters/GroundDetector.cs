using System;
using System.Collections.Generic;
using UnityEngine;

namespace LD59.ExtractMoles.Characters
{
   [RequireComponent( typeof(SphereCollider) )]
   public class GroundDetector : MonoBehaviour
   {
      private readonly List<Collider> _colliders = new();

      public bool Grounded => _colliders.Count > 0;

      private void OnTriggerEnter( Collider other )
      {
         if(other.isTrigger) return;

         if(_colliders.Contains( other )) return;
         _colliders.Add( other );
      }

      private void OnTriggerExit( Collider other ) => _colliders.Remove( other );

      private void Update()
      {
         for(var i = _colliders.Count - 1; i >= 0; --i)
         {
            if(!_colliders[ i ].enabled)
            {
               _colliders.RemoveAt( i );
            }
         }
      }
   }
}