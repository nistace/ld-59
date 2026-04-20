using LD59.ExtractMoles.Utilities;
using TMPro;
using UnityEngine;

namespace LD59.ExtractMoles.Cutscenes
{
   public class CutsceneTextManager : MonoBehaviour
   {
      [SerializeField] private CutsceneSharedSettings _settings;
      [SerializeField] private TMP_Text _previousText;
      [SerializeField] private TMP_Text _text;

      private bool HasText { get; set; }

      public void HideText()
      {
         if(!HasText) return;
         _previousText.transform.position = _text.transform.position;
         _previousText.text = _text.text;
         _previousText.color = _text.color;
         _text.color = _text.color.Alpha( 0 );

         HasText = false;
      }

      public void ShowText( string line, Color color )
      {
         HideText();

         HasText = true;

         _text.text = line;
         _text.color = color.Alpha( 0 );
      }

      private void Update()
      {
         _previousText.transform.position += Vector3.up * (_settings.PreviousTextSpeed * Time.deltaTime);
         _previousText.color = _previousText.color.MoveAlpha( -Time.deltaTime * _settings.PreviousTextOpacityChange );
         if(HasText)
         {
            _text.color = _text.color.MoveAlpha( Time.deltaTime * _settings.NewTextOpacityChange );
         }
      }

      public void CleanUp()
      {
         _previousText.text = string.Empty;
         _previousText.color = _previousText.color.Alpha( 0 );
         _text.text = string.Empty;
         _text.color = _text.color.Alpha( 0 );
      }
   }
}