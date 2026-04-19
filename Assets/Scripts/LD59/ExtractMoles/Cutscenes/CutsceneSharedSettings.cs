using UnityEngine;

namespace LD59.ExtractMoles.Cutscenes
{
   [CreateAssetMenu]
   public class CutsceneSharedSettings : ScriptableObject
   {
      [SerializeField] private float _boxFadeInDuration = .5f;
      [SerializeField] private float _boxFadeOutDuration = .5f;
      [SerializeField] private float _puppetsMovementSpeed = 6f;
      [SerializeField] private float _puppetsRotationsSpeed = 60f;
      [SerializeField] private float _previousTextSpeed = 2;
      [SerializeField] private float _previousTextOpacityChange = .5f;
      [SerializeField] private float _newTextOpacityChange = .1f;
      [SerializeField] private Color[] _charactersColor;

      public float BoxFadeInDuration => _boxFadeInDuration;
      public float BoxFadeOutDuration => _boxFadeOutDuration;
      public float PuppetsMovementSpeed => _puppetsMovementSpeed;
      public float PuppetsRotationSpeed => _puppetsRotationsSpeed;
      public float PreviousTextSpeed => _previousTextSpeed;
      public float PreviousTextOpacityChange => _previousTextOpacityChange;
      public float NewTextOpacityChange => _newTextOpacityChange;

      public Color CharacterColor( int index ) => _charactersColor[ index ];
   }
}