using System.Collections.Generic;
using UnityEngine;

namespace LD59.ExtractMoles.Mechanisms
{
   public class ElevatorPlatformContainer : MonoBehaviour
   {
      private readonly HashSet<Transform> _content = new();

      public IReadOnlyCollection<Transform> Content => _content;

      private void OnTriggerEnter( Collider other )
      {
         _content.Add( other.transform.root );
      }

      private void OnTriggerExit( Collider other )
      {
         _content.Remove( other.transform.root );
      }
   }
}