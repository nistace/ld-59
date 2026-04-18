using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace LD59.ExtractMoles.Utilities
{
   public static class SpawnWithScale
   {
      public static async UniTask Play( Transform target, Data data )
      {
         target.localScale = Vector3.one * data.ScaleCurve.Evaluate( 0 );

         for(var progress = 0f; progress < 1; progress += Time.deltaTime / data.Speed)
         {
            target.localScale = Vector3.one * data.ScaleCurve.Evaluate( progress );

            await UniTask.NextFrame();
         }

         target.localScale = Vector3.one * data.ScaleCurve.Evaluate( 1 );
      }

      [Serializable]
      public class Data
      {
         [SerializeField] private AnimationCurve _scaleCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
         [SerializeField] private float _speed = .5f;

         public Data() { }

         public Data( float speed )
         {
            _speed = speed;
         }

         public AnimationCurve ScaleCurve => _scaleCurve;
         public float Speed => _speed;
      }
   }
}