using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Utilities;
using System.Collections.Generic;
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
      [SerializeField] private Color _gizmoColor = Color.white;
      [SerializeField] private SpawnWithScaleConfigurationData _despawnData;

      public Color GizmoColor => _gizmoColor;

      public bool IsAboutToHitSomething( float speed ) => IsAboutToHitSomething( speed, out _, out _ );

      public bool IsAboutToHitSomething( float speed, out IReadOnlyList<Collider> colliders, out int hits )
      {
         colliders = Colliders;
         hits = Physics.OverlapSphereNonAlloc( transform.position + Vector3.up * (_collider.height * .5f) + transform.forward * (speed * Time.deltaTime),
            _collider.radius,
            Colliders,
            _collisionMask );

         if((_collisionMask.value & (1 << transform.gameObject.layer)) == 0)
         {
            return hits > 0;
         }

         return Colliders.Take( hits ).Any( hitCollider => !hitCollider.gameObject.transform.IsChildOf( transform ) );
      }

      public async UniTask Despawn()
      {
         foreach(var toNotify in GetComponentsInChildren<INotifiedOfCharacterDespawn>())
         {
            toNotify.OnDespawn();
         }

         await SpawnWithScale.Play( transform, _despawnData.Data );

         Destroy( gameObject );
      }
   }
}