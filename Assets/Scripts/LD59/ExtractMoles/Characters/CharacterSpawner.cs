using Cysharp.Threading.Tasks;
using LD59.ExtractMoles.Utilities;
using UnityEngine;

namespace LD59.ExtractMoles.Characters
{
   public class CharacterSpawner : MonoBehaviour
   {
      [SerializeField] private Character _characterPrefab;
      [SerializeField] private SpawnWithScaleConfigurationData _spawnWithScaleDate;

      public Color GizmoColor => _characterPrefab.GizmoColor;

      public async UniTask<Character> SpawnAsync()
      {
         var instance = Instantiate( _characterPrefab, transform.position, transform.rotation );

         await SpawnWithScale.Play( instance.transform, _spawnWithScaleDate.Data );

         return instance;
      }
   }
}