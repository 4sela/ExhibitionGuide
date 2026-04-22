using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;
using Game.Systems.Minigames;
using Game.Systems.Narrative.Events;
using Game.Systems.Narrative.Runtime;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseGameController : MonoBehaviour, IMinigame
    {
        [Header("Modals")]
        [SerializeField] private GameObject tutorialModal;
        [SerializeField] private GameResult gameResult;

        [Header("UI")]
        [SerializeField] private GameObject morseGameContainer;
        [SerializeField] private TextMeshProUGUI previewSymbol;
        [SerializeField] private TextMeshProUGUI previewLetter;
        [SerializeField] private TextMeshProUGUI currentMorseText;
        [SerializeField] private TextMeshProUGUI[] letterTiles;
        [SerializeField] private Image progressBar;
        [SerializeField] private MorseInputHandler input;

        private MorseSequenceBuilder _builder;
        private MorseWordValidator _validator;

        private int _currentTileIndex = 0;

        void Awake()
        {
            Initialise();
        }

        void Start()
        {
            input.OnHolding += OnHolding;
            input.OnSymbolDetected += OnSymbolDetected;
            _builder.OnLetterFinalized += OnLetterFinalized;
            _builder.OnInvalidSequence += OnInvalidSequence;
        }

        void Update()
        {
            _builder.Tick(Time.time);
            CheckWord();
        }

        private void Initialise()
        {
            tutorialModal.SetActive(true);
            _builder = MorseSequenceBuilder.Instance;
            _validator = new MorseWordValidator("HER");
            ResetGameState();
        }

        private void ReInitialise()
        {
            _builder = MorseSequenceBuilder.Instance;
            _validator = new MorseWordValidator("HER");
            ResetGameState();
        }

        public void ResetGameState()
        {
            _builder.ClearData();

            ClearCurrentInput();

            _currentTileIndex = 0;

            foreach (TextMeshProUGUI tile in letterTiles)
            {
                tile.text = "";
            }
        }

        private void OnPreviewSymbol(char symbol)
        {
            // Show symbol preview
            previewSymbol.text = symbol == '.' ? "•" : "—";

            // Simulate next state
            string previewSequence = _builder.GetCurrentSymbols() + symbol;

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
            previewLetter.SetText(_builder.GetCurrentLetter().ToString());
        }

        private void OnHolding(float duration)
        {
            // Logic for progressbar
            float progress = Mathf.Clamp01(duration / input.dotThreshold);
            progressBar.fillAmount = progress;

            // Logic for symbolpreview
            char symbol = duration < input.dotThreshold ? '.' : '-';

            previewSymbol.text = symbol == '.' ? "•" : "—";

            // Simulate next state
            string previewSequence = _builder.GetCurrentSymbols() + symbol;

            if (MorseTranslator.TryTranslate(previewSequence, out char letter))
            {
                previewLetter.text = letter.ToString();
            }

            else
            {
                previewLetter.text = "";
            }
        }

        private void OnSymbolDetected(char symbol)
        {
            progressBar.fillAmount = 0f;
            string current = _builder.GetCurrentSymbols();
            currentMorseText.text = FormatMorse(current);

            char letter = _builder.GetCurrentLetter();
            previewLetter.text = letter == ' ' ? "" : letter.ToString();

            // Debug.Log($"Input: {symbol}");
            Debug.Log($"Current Sequence: {_builder.GetCurrentSymbols()}");
        }

        private void OnLetterFinalized(char letter)
        {
            if (_currentTileIndex < letterTiles.Length)
            {
                letterTiles[_currentTileIndex].text = letter.ToString();
                _currentTileIndex++;
            }

            ClearCurrentInput();
            Debug.Log($"Letter: {letter}");
        }

        public void CheckWord()
        {
            char[] letters = _builder.DecodedLetters.ToArray();
            bool correct = _validator.Check(letters);

            if (correct)
            {
                gameResult.IsCompleted = correct;
                gameResult.SummonResult();
            }
        }

        private void OnInvalidSequence(string sequence)
        {
            Debug.Log("Invalid Morse: " + sequence);

            ClearCurrentInput();

            // Optional: feedback
            previewLetter.text = "✖"; // quick visual error
            currentMorseText.text = "Invalid letter";

            // Reset builder state
            _builder.ResetCurrentSequenceOnly();
        }


        private string FormatMorse(string raw)
        {
            return string.Join(" ",
                raw.Replace(".", "•")
                   .Replace("-", "—")
                   .ToCharArray()
            );
        }

        private void ClearCurrentInput()
        {
            currentMorseText.text = "";
            previewSymbol.text = "";
            previewLetter.text = "";
            progressBar.fillAmount = 0f;
        }

        public void ProceedToNarrativeScreen()
        {
            NarrativeManager.Instance.ContinueDefault();
        }
    }
}
