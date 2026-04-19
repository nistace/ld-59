using System;
using UnityEngine;

namespace LD59.ExtractMoles.Cutscenes
{
   [CreateAssetMenu]
   public class CutsceneScript : ScriptableObject
   {
      [SerializeField] private CharacterChange[] _initialCharacterPositions;
      [SerializeField] private float _delayBefore = .1f;
      [SerializeField] private Line[] _lines;
      [SerializeField] private float _delayAfter = .1f;

      public float DelayBefore => _delayBefore;
      public Line[] Lines => _lines;
      public CharacterChange[] InitialCharacterPositions => _initialCharacterPositions;
      public float DelayAfter => _delayAfter;

      [Serializable]
      public class Line
      {
         [SerializeField] private string _text;
         [SerializeField] private CutsceneEvent.Type _eventBefore;
         [SerializeField] private CharacterChange[] _characterChanges;
         [SerializeField] private float _delayBefore = .1f;
         [SerializeField] private float _delayAfter = .1f;
         [SerializeField] private AudioClip _clip;
         [SerializeField] private CutsceneCharacter _character;
         [SerializeField] private bool _endWithAShrug;

         public CharacterChange[] CharacterChanges => _characterChanges;
         public float DelayBefore => _delayBefore;
         public float DelayAfter => _delayAfter;
         public AudioClip Clip => _clip;
         public string Text => _text;
         public CutsceneCharacter Character => _character;
         public CutsceneEvent.Type EventBefore => _eventBefore;
         public bool EndWithAShrug => _endWithAShrug;
      }

      [Serializable]
      public class CharacterChange
      {
         [SerializeField] private CutsceneCharacter _character;
         [SerializeField] private CutsceneCharacterPosition _characterPosition;
         [SerializeField] private bool _await = true;

         public CutsceneCharacter Character => _character;
         public CutsceneCharacterPosition CharacterPosition => _characterPosition;
         public bool Await => _await;
      }
   }
}