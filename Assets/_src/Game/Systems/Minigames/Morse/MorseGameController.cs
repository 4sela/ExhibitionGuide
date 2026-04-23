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
        [Header("Game Settings")]
        [SerializeField] private string targetWord = "RAY";

        [Header("System Dependencies")]
        [SerializeField] private MorseInputHandler morseInputHandler;
        [SerializeField] private GameResult gameResult;

        [Header("UI Screens & Containers")]
        [SerializeField] private GameObject tutorialModal;
        [SerializeField] private GameObject morseGameContainer;

        [Header("UI Live Input Feedback")]
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI previewSymbol;
        [SerializeField] private TextMeshProUGUI previewLetter;

        [Header("UI Decoded Progress")]
        [SerializeField] private TextMeshProUGUI currentMorseText;
        [SerializeField] private TextMeshProUGUI[] letterTiles;

        private MorseSequenceBuilder _morseSeqBuilder;
        private MorseWordValidator _morseWordValidator;

        private int _currentTileIndex = 0;

        void Awake()
        {
            _morseSeqBuilder = new MorseSequenceBuilder();
            _morseWordValidator = new MorseWordValidator(targetWord);
            AudioManager.Instance.Voice.StopVoice();
        }

        void Start()
        {
            tutorialModal.SetActive(true);
            ResetGameState();
        }

        void OnEnable()
        {
            // NOTE: Firstly, we link Input to Builder (We remove Singleton weirdness)
            morseInputHandler.OnPressStateChanged += _morseSeqBuilder.SetPressing;
            morseInputHandler.OnSymbolAdded += _morseSeqBuilder.AddSymbol;

            // NOTE: Secondly, we link Input to UI
            morseInputHandler.OnHolding += OnHolding;
            morseInputHandler.OnSymbolAdded += OnSymbolAdded;

            // NOTE: Thirdly, we link Builder to Game Logic
            _morseSeqBuilder.OnLetterFormed += OnLetterFormed;
            _morseSeqBuilder.OnUnrecognizedMorseCharacter += OnUnrecognizedMorseCharacter;
        }

        void OnDisable()
        {
            morseInputHandler.OnPressStateChanged -= _morseSeqBuilder.SetPressing;
            morseInputHandler.OnSymbolAdded -= _morseSeqBuilder.AddSymbol;

            morseInputHandler.OnHolding -= OnHolding;
            morseInputHandler.OnSymbolAdded -= OnSymbolAdded;

            _morseSeqBuilder.OnLetterFormed -= OnLetterFormed;
            _morseSeqBuilder.OnUnrecognizedMorseCharacter -= OnUnrecognizedMorseCharacter;
        }

        void Update()
        {
            _morseSeqBuilder.Tick(Time.time);
        }

        public void ResetGameState()
        {
            _morseSeqBuilder.ClearData();
            ClearCurrentInput();
            _currentTileIndex = 0;

            foreach (TextMeshProUGUI tile in letterTiles)
                tile.text = "";
        }

        /// <remarks>
        /// When holding the flashlight.
        /// </remarks>
        private void OnHolding(float progress, char predictedSymbol)
        {
            progressBar.fillAmount = progress;
            previewSymbol.text = (predictedSymbol == '.') ? "•" : "—";

            string previewSequence = _morseSeqBuilder.CurrentSymbolSequence + predictedSymbol;

            if (MorseTranslator.TryTranslate(previewSequence, out char letter))
                previewLetter.text = letter.ToString();
            else
                previewLetter.text = "";
        }

        /// <remarks>
        /// When a symbol has been added.
        /// </remarks>
        private void OnSymbolAdded(char symbol, float time)
        {
            progressBar.fillAmount = 0f;
            currentMorseText.text = FormatMorse(_morseSeqBuilder.CurrentSymbolSequence);

            char letter = _morseSeqBuilder.GetPreviewLetter();
            previewLetter.text = letter == ' ' ? "" : letter.ToString();
        }

        /// <remarks>
        /// When a sequence of symbols have been succesfully translated to a letter.
        /// </remarks>
        private void OnLetterFormed(char letter)
        {
            if (_currentTileIndex < letterTiles.Length)
            {
                letterTiles[_currentTileIndex].text = letter.ToString();
                _currentTileIndex++;
            }

            ClearCurrentInput();

            // If all 3 tiles are filled
            if (_currentTileIndex == letterTiles.Length)
                ValidateTargetWord();
        }

        /// <remarks>
        /// When the typed dots and dashes typed DON'T exist in the Morse code dictionary!!
        /// </remarks>
        private void OnUnrecognizedMorseCharacter()
        {
            ClearCurrentInput();
            currentMorseText.text = "Invalid letter";
            _morseSeqBuilder.ResetCurrentSequenceOnly();
        }

        public void ValidateTargetWord()
        {
            char[] letters = _morseSeqBuilder.DecodedLetters.ToArray();
            bool isCorrect = _morseWordValidator.Validate(letters);

            gameResult.IsCompleted = isCorrect;
            gameResult.SummonResult();
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
