using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD59
{
   public class MenuStartButton : MonoBehaviour
   {
      public void StartGame() => SceneManager.LoadScene( "Game" );
   }
}