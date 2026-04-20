using UnityEngine;

namespace LD59.ExtractMoles
{
   public class MusicPlayer : MonoBehaviour
   {
      private static MusicPlayer _instance;

      private void Awake()
      {
         if(_instance != null && _instance != this)
         {
            Destroy( gameObject );
            return;
         }

         _instance = this;
         DontDestroyOnLoad( gameObject );
      }
   }
}