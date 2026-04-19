using UnityEngine;

namespace LD59.ExtractMoles.Environment
{
   public class Rotator : MonoBehaviour
   {
      [SerializeField] private Vector3 _rotationOverTime;

      private void Update()
      {
         transform.Rotate( _rotationOverTime * Time.deltaTime );
      }
   }
}