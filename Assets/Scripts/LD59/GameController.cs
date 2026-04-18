using Cysharp.Threading.Tasks;
using LD59.Levels;
using System;
using UnityEngine;

namespace LD59
{
   public class GameController : MonoBehaviour
   {
      [SerializeField] private LevelManager _levelManager;

      private async void Start()
      {
         try
         {
            _levelManager.OnCurrentLevelEnded.AddListener( HandleCurrentLevelEnded );
            await _levelManager.GoToNextLevel();
         }
         catch(Exception e)
         {
            Debug.LogException( e );
         }
      }

      private void OnDestroy()
      {
         _levelManager.OnCurrentLevelEnded.RemoveListener( HandleCurrentLevelEnded );
      }

      private void HandleCurrentLevelEnded()
      {
         _levelManager.GoToNextLevel().Forget();
      }
   }
}