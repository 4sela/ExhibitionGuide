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
        [SerializeField] private string targetWord = "HER";

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

        private MorseSequenceBuilder _morseSeqbuilder;
        private MorseWordValidator _morseWordValidator;

        private int _currentTileIndex = 0;

        void Awake()
        {
            _morseSeqbuilder = new MorseSequenceBuilder();
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
            morseInputHandler.OnPressStateChanged += _morseSeqbuilder.SetPressing;
            morseInputHandler.OnSymbolAdded += _morseSeqbuilder.AddSymbol;

            // NOTE: Secondly, we link Input to UI
            morseInputHandler.OnHolding += OnHolding;
            morseInputHandler.OnSymbolAdded += OnSymbolDetected;

            // NOTE: Thirdly, we link Builder to Game Logic
            _morseSeqbuilder.OnLetterFinalized += OnLetterFinalized;
            _morseSeqbuilder.OnInvalidSequence += OnInvalidSequence;
        }

        void OnDisable()
        {
            morseInputHandler.OnPressStateChanged -= _morseSeqbuilder.SetPressing;
            morseInputHandler.OnSymbolAdded -= _morseSeqbuilder.AddSymbol;

            morseInputHandler.OnHolding -= OnHolding;
            morseInputHandler.OnSymbolAdded -= OnSymbolDetected;

            _morseSeqbuilder.OnLetterFinalized -= OnLetterFinalized;
            _morseSeqbuilder.OnInvalidSequence -= OnInvalidSequence;
        }

        void Update()
        {
            _morseSeqbuilder.Tick(Time.time);
        }

        /// <remarks>
        /// Call by retry button in Lose modal.
        /// </remarks>
        public void RetryGame()
        {
            ResetGameState();
        }

        public void ResetGameState()
        {
            _morseSeqbuilder.ClearData();
            ClearCurrentInput();
            _currentTileIndex = 0;

            foreach (TextMeshProUGUI tile in letterTiles)
                tile.text = "";
        }

        private void OnHolding(float progress, char predictedSymbol)
        {
            progressBar.fillAmount = progress;
            previewSymbol.text = predictedSymbol == '.' ? "•" : "—";

            string previewSequence = _morseSeqbuilder.CurrentSymbolSequence + predictedSymbol;

            if (MorseTranslator.TryTranslate(previewSequence, out char letter))
                previewLetter.text = letter.ToString();
            else
                previewLetter.text = "";
        }

        private void OnSymbolDetected(char symbol, float time)
        {
            progressBar.fillAmount = 0f;
            currentMorseText.text = FormatMorse(_morseSeqbuilder.CurrentSymbolSequence);

            char letter = _morseSeqbuilder.GetCurrentLetter();
            previewLetter.text = letter == ' ' ? "" : letter.ToString();
        }

        private void OnLetterFinalized(char letter)
        {
            if (_currentTileIndex < letterTiles.Length)
            {
                letterTiles[_currentTileIndex].text = letter.ToString();
                _currentTileIndex++;
            }

            ClearCurrentInput();
            CheckWord();
        }

        public void CheckWord()
        {
            Debug.Log("CheckWord(); <-- Called");
            char[] letters = _morseSeqbuilder.DecodedLetters.ToArray();
            bool correct = _morseWordValidator.Check(letters);

            if (correct)
            {
                gameResult.IsCompleted = correct;
                gameResult.SummonResult();
            }
        }

        private void OnInvalidSequence(string sequence)
        {
            ClearCurrentInput();
            currentMorseText.text = "Invalid letter";
            _morseSeqbuilder.ResetCurrentSequenceOnly();
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
