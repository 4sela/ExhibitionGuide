using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class MorseGameController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI previewText;
    [SerializeField] private TextMeshProUGUI currentMorseText;
    [SerializeField] private TextMeshProUGUI[] letterTiles;

    [SerializeField] private MorseInputHandler input;

    private MorseSequenceBuilder builder;
    private MorseWordValidator validator;

    private int currentTileIndex = 0;

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

        previewText.SetText(builder.GetCurrentLetter().ToString());

    }

    private void OnHolding(float duration)
    {

        //Change image to Lit flashlight
    }

    private void OnSymbolDetected(char symbol)
    {

        builder.AddSymbol(symbol, Time.time);

        currentMorseText.text = FormatMorse(builder.GetCurrentSymbols());

        Debug.Log($"Input: {symbol}");
        Debug.Log($"Current Sequence: {builder.GetCurrentSymbols()}");
    }

    private void OnLetterFinalized(char letter)
    {
        if (currentTileIndex < letterTiles.Length)
        {
            letterTiles[currentTileIndex].text = letter.ToString();
            currentTileIndex++;
        }

        currentMorseText.text = "";

        Debug.Log($"Letter: {letter}");
    }

    public void CheckWord()
    {
        builder.FinalizeLetter();

        var letters = builder.DecodedLetters.ToList();
        bool correct = validator.Check(letters);

        Debug.Log(correct ? "Correct!" : "Wrong");

        // Reset UI
        previewText.text = "";
        currentTileIndex = 0;

        foreach (var tile in letterTiles)
            tile.text = "";

        builder.Reset();
    }


    private string FormatMorse(string raw)
    {
        return string.Join(" ",
            raw.Replace(".", "•")
               .Replace("-", "—")
               .ToCharArray()
        );
    }
}