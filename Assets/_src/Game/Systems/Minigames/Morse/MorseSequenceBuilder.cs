using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseSequenceBuilder
    {
        public event Action<char> OnLetterFormed;
        public event Action OnUnrecognizedMorseCharacter;

        private List<char> _decodedLetters = new();
        private string _currentSymbolSequence = "";
        private float _lastInputTime = float.MaxValue;
        private bool _isPressing = false;

        private readonly float letterPauseThreshold = 1f;

        public IReadOnlyList<char> DecodedLetters => _decodedLetters;
        public string CurrentSymbolSequence => _currentSymbolSequence;

        public void AddSymbol(char symbol, float currentTime)
        {
            _currentSymbolSequence += symbol;
            _lastInputTime = currentTime;
        }

        public void SetPressing(bool pressing)
        {
            _isPressing = pressing;
        }

        public void Tick(float currentTime)
        {
            if (string.IsNullOrEmpty(_currentSymbolSequence))
                return;

            if (!_isPressing && currentTime - _lastInputTime > letterPauseThreshold)
                AttemptLetterTranslation();
        }

        private void AttemptLetterTranslation()
        {
            if (string.IsNullOrEmpty(_currentSymbolSequence))
                return;

            bool isValidLetter = MorseTranslator.TryTranslate(_currentSymbolSequence, out char letter);

            if (isValidLetter)
            {
                _decodedLetters.Add(letter);
                OnLetterFormed?.Invoke(letter);
            }

            else
                OnUnrecognizedMorseCharacter?.Invoke();

            ResetCurrentSequenceOnly();
        }

        public char GetPreviewLetter()
        {
            char preview = ' ';

            if (MorseTranslator.TryTranslate(_currentSymbolSequence, out char letter))
                preview = letter;

            return preview;
        }

        public void ClearData()
        {
            _decodedLetters.Clear();
            ResetCurrentSequenceOnly();
        }

        public void ResetCurrentSequenceOnly()
        {
            _currentSymbolSequence = "";
            _lastInputTime = float.MaxValue;
        }
    }
}
