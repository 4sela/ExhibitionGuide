using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;
using Game.Systems.Minigames;
using Game.Systems.Haptics;
using Game.Systems.Narrative.Events;
using Game.Systems.Narrative.Runtime;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseGameController : MonoBehaviour, IMinigame
    {
        [Header("Game Settings")]
        [SerializeField] private string targetWord = "RAY";
        [SerializeField] private float letterTileAlpha = 0.4f;

        [Header("System Dependencies")]
        [SerializeField] private MorseInputHandler morseInputHandler;
        [SerializeField] private GameResult gameResult;

        [Header("UI Screens & Containers")]
        [SerializeField] private GameObject tutorialModal;
        [SerializeField] private GameObject morseGameContainer;

        [Header("UI Live Input Feedback")]
        [SerializeField] private Image progressBar;

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

        public void ResetLastLetter()
        {
            _morseSeqBuilder.ResetCurrentSequenceOnly();
            ClearCurrentInput();

            if (!_morseSeqBuilder.ResetLastLetter())
                return;

            if (_currentTileIndex <= 0)
                return;

            _currentTileIndex--;
            letterTiles[_currentTileIndex].text = "";
        }

        /// <remarks>
        /// When holding the flashlight.
        /// </remarks>
        private void OnHolding(float progress, char predictedSymbol)
        {
            progressBar.fillAmount = progress;
            letterTiles[_currentTileIndex].alpha = letterTileAlpha;
        }

        /// <remarks>
        /// When a symbol has been added.
        /// </remarks>
        private void OnSymbolAdded(char symbol, float time)
        {
            if (symbol == '.')
                HapticsService.PlayTick();
            else
                HapticsService.PlayClick();

            progressBar.fillAmount = 0f;
            currentMorseText.text = FormatMorse(_morseSeqBuilder.CurrentSymbolSequence);

            char letter = _morseSeqBuilder.GetPreviewLetter();
            letterTiles[_currentTileIndex].text = letter == ' ' ? "" : letter.ToString();
        }

        /// <remarks>
        /// When a sequence of symbols have been succesfully translated to a letter.
        /// </remarks>
        private void OnLetterFormed(char letter)
        {
            letterTiles[_currentTileIndex].alpha = 1f;

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
            HapticsService.PlayError();
            ClearCurrentInput();
            currentMorseText.text = "";
            _morseSeqBuilder.ResetCurrentSequenceOnly();
            //Maybe make the CurrentSequence field, turn red for color indication also.
        }

        public void ValidateTargetWord()
        {
            char[] letters = _morseSeqBuilder.DecodedLetters.ToArray();
            bool isCorrect = _morseWordValidator.Validate(letters);

            gameResult.IsCompleted = isCorrect;

            if (isCorrect)
                HapticsService.PlaySuccess();
            else
                HapticsService.PlayError();

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
            progressBar.fillAmount = 0f;
        }

        public void ProceedToNarrativeScreen()
        {
            NarrativeManager.Instance.ContinueDefault();
        }
    }
}
