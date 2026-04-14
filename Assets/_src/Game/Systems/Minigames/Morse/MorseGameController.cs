using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class MorseGameController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI previewSymbol;
    [SerializeField] private TextMeshProUGUI previewLetter;
    [SerializeField] private TextMeshProUGUI currentMorseText;
    [SerializeField] private TextMeshProUGUI[] letterTiles;
    [SerializeField] private Image progressBar;
    [SerializeField] private MorseInputHandler input;

    private MorseSequenceBuilder builder;
    private MorseWordValidator validator;

    private int currentTileIndex = 0;
    [SerializeField] private float dotThreshold = 0.35f; //Has to match dotThreshold from inputhandler

    private void Awake()
    {
        builder = MorseSequenceBuilder.Instance;
        validator = new MorseWordValidator("HER");
    }

    private void Start()
    {
        input.OnPreviewSymbol += OnPreviewSymbol;
        input.OnHolding += OnHolding;
        input.OnSymbolDetected += OnSymbolDetected;
        builder.OnLetterFinalized += OnLetterFinalized;
        builder.OnInvalidSequence += OnInvalidSequence;
    }

    private void Update()
    {
        builder.Tick(Time.time);
    }

    private void OnPreviewSymbol(char symbol)
    {
        //Show symbol preview
        previewSymbol.text = symbol == '.' ? "•" : "—";

        //simulate next state
        string previewSequence = builder.GetCurrentSymbols() + symbol;

        if (MorseTranslator.TryTranslate(previewSequence, out char letter))
        {
            previewLetter.text = letter.ToString();
        }
        else
        {
            previewLetter.text = "";
        }
    }

    private void OnPreviewLetter(char letter) 
    {
        previewLetter.SetText(builder.GetCurrentLetter().ToString());
    }

    private void OnHolding(float duration)
    {

       float progress = Mathf.Clamp01(duration / dotThreshold);
       progressBar.fillAmount = progress;

       //progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.deltaTime * 10f);

       //Change color when filling
       //progressBar.color = duration < dotThreshold ? Color.white : Color.red;
    }

    private void OnSymbolDetected(char symbol)
    {
        progressBar.fillAmount = 0f;
        string current = builder.GetCurrentSymbols();
        currentMorseText.text = FormatMorse(current);

        char letter = builder.GetCurrentLetter();
        previewLetter.text = letter == ' ' ? "" : letter.ToString();

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

        //CLEAR EVERYTHING RELATED TO CURRENT INPUT
        currentMorseText.text = "";
        previewSymbol.text = "";
        previewLetter.text = "";
        progressBar.fillAmount = 0f;

        Debug.Log($"Letter: {letter}");
    }

    public void CheckWord()
    {
        builder.FinalizeLetter();

        var letters = builder.DecodedLetters.ToList();
        bool correct = validator.Check(letters);

        Debug.Log(correct ? "Correct!" : "Wrong");

        // Reset UI
        previewSymbol.text = "";
        currentTileIndex = 0;

        foreach (var tile in letterTiles)
            tile.text = "";

        builder.Reset();
    }

    private void OnInvalidSequence(string sequence)
    {
        Debug.Log("Invalid Morse: " + sequence);

        currentMorseText.text = "";
        previewSymbol.text = "";
        previewLetter.text = "";

        // Optional: feedback
        previewLetter.text = "✖"; // quick visual error
        currentMorseText.text = "Invalid letter";

        // Reset builder state
        builder.ResetCurrentSequenceOnly();
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