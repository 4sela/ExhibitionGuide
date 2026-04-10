using UnityEngine;
using System.Linq;

public class MorseGameController : MonoBehaviour
{
    [SerializeField] private MorseInputHandler input;

    private MorseSequenceBuilder builder;
    private MorseWordValidator validator;

    private void Awake()
    {
        builder = new MorseSequenceBuilder();
        validator = new MorseWordValidator("HER");
    }

    private void Start()
    {
        input.OnPreviewSymbol += OnPreviewSymbol;
        input.OnHolding += OnHolding;
        input.OnSymbolDetected += OnSymbolDetected;

        builder.OnLetterFinalized += OnLetterFinalized;
    }

    private void Update()
    {
        builder.Tick(Time.time);
    }

    private void OnPreviewSymbol(char symbol)
    {
        Debug.Log($"Preview: {symbol}");
        // UI → show live dot/dash
    }

    private void OnHolding(float duration)
    {
        // UI → progress bar if you want
    }

    private void OnSymbolDetected(char symbol)
    {
        builder.AddSymbol(symbol, Time.time);

        Debug.Log($"Input: {symbol}");
        Debug.Log($"Current Sequence: {builder.GetCurrentSymbols()}");
    }

    private void OnLetterFinalized(char letter)
    {
        Debug.Log($"Letter: {letter}");
    }

    public void CheckWord()
    {
        builder.FinalizeLetter();

        var letters = builder.DecodedLetters.ToList();

        bool correct = validator.Check(letters);

        Debug.Log(correct ? "Correct!" : "Wrong");

        builder.Reset();
    }
}