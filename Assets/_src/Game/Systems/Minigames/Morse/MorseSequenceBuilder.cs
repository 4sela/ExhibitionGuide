using System;
using System.Collections.Generic;
using UnityEngine;

public class MorseSequenceBuilder
{
    public static MorseSequenceBuilder Instance = new();

    public event Action<char> OnLetterFinalized;
    public event Action<string> OnInvalidSequence;

    private List<char> _decodedLetters = new();
    private string _currentSymbolSequence = "";
    private float _lastInputTime = float.MaxValue;
    private bool _isPressing = false;
    private readonly float letterPauseThreshold = 1f;

    public IReadOnlyList<char> DecodedLetters => _decodedLetters;

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
        {
            FinalizeLetter();
        }
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
            //INVALID INPUT
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
        {
            preview = letter;
        }
        return preview;
    }

    public string GetCurrentSymbols()
    {
        return _currentSymbolSequence;
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
