using System;
using System.Collections.Generic;

public class MorseSequenceBuilder
{
    private string currentSymbolSequence = "";
    private List<char> decodedLetters = new();

    private float lastInputTime = float.MaxValue;

    private readonly float letterPauseThreshold = 0.6f;

    public IReadOnlyList<char> DecodedLetters => decodedLetters;

    public event Action<char> OnLetterFinalized;

    public void AddSymbol(char symbol, float currentTime)
    {
        currentSymbolSequence += symbol;
        lastInputTime = currentTime;
    }

    public void Tick(float currentTime)
    {
        if (string.IsNullOrEmpty(currentSymbolSequence))
            return;

        if (currentTime - lastInputTime > letterPauseThreshold)
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

        currentSymbolSequence = "";
        lastInputTime = float.MaxValue;
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
}