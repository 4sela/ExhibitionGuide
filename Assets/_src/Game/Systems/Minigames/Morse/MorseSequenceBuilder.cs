using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseSequenceBuilder
    {
        public event Action<char> OnLetterFinalized;
        public event Action<string> OnInvalidSequence;

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
                FinalizeLetter();
        }

        public void FinalizeLetter()
        {
            if (string.IsNullOrEmpty(_currentSymbolSequence))
                return;

            if (MorseTranslator.TryTranslate(_currentSymbolSequence, out char letter))
            {
                _decodedLetters.Add(letter);
                OnLetterFinalized?.Invoke(letter);
            }
            else
            {
                OnInvalidSequence?.Invoke(_currentSymbolSequence);
                _currentSymbolSequence = "";
            }

            _currentSymbolSequence = "";
            _lastInputTime = float.MaxValue;
        }

        public char GetCurrentLetter()
        {
            char preview = ' ';

            if (MorseTranslator.TryTranslate(_currentSymbolSequence, out char letter))
                preview = letter;

            return preview;
        }

        public void ClearData()
        {
            _currentSymbolSequence = "";
            _decodedLetters.Clear();
            _lastInputTime = float.MaxValue;
        }

        public void ResetCurrentSequenceOnly()
        {
            _currentSymbolSequence = "";
            _lastInputTime = float.MaxValue;
        }
    }
}
