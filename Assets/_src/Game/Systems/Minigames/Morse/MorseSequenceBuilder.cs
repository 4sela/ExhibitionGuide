using System;
using System.Collections.Generic;
using UnityEngine;

public class MorseSequenceBuilder
{
    public static MorseSequenceBuilder Instance = new ();


    private string currentSymbolSequence = "";
    private List<char> decodedLetters = new();

    private float lastInputTime = float.MaxValue;

    private readonly float letterPauseThreshold = 1.5f;
    private bool isPressing = false;


    public IReadOnlyList<char> DecodedLetters => decodedLetters;

    public event Action<char> OnLetterFinalized;
    public event Action<string> OnInvalidSequence;

    public void AddSymbol(char symbol, float currentTime)
    {
        currentSymbolSequence += symbol;
        lastInputTime = currentTime;
    }

    public void SetPressing(bool pressing)
    {
        isPressing = pressing;
    }

    public void Tick(float currentTime)
    {
        if (string.IsNullOrEmpty(currentSymbolSequence))
            return;

        if (!isPressing && currentTime - lastInputTime > letterPauseThreshold)
        {
            FinalizeLetter();
        }
    }

    public void FinalizeLetter()
    {
        if (string.IsNullOrEmpty(currentSymbolSequence))
            return;

        if (MorseTranslator.TryTranslate(currentSymbolSequence, out char letter))
        {
            decodedLetters.Add(letter);
            OnLetterFinalized?.Invoke(letter);
        }

        else
        {
            //INVALID INPUT
            OnInvalidSequence?.Invoke(currentSymbolSequence);
            currentSymbolSequence = "";
        }

        currentSymbolSequence = "";
        lastInputTime = float.MaxValue;
    }

    public char GetCurrentLetter()
    {
        char preview = ' ';

        if (MorseTranslator.TryTranslate(currentSymbolSequence, out char letter))
        {
            preview = letter;
        }
        return preview;
    }

    public string GetCurrentSymbols()
    {
        return currentSymbolSequence;
    }

    public void Reset()
    {
        currentSymbolSequence = "";
        decodedLetters.Clear();
        lastInputTime = float.MaxValue;
    }

    public void ResetCurrentSequenceOnly()
    {
        currentSymbolSequence = "";
        lastInputTime = float.MaxValue;
    }

}