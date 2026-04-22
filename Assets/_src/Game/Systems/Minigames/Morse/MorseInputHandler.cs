using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Game.Systems.Minigames.Morse;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseInputHandler : MonoBehaviour
    {
        [SerializeField] private float _dotThreshold = 0.35f;

        public event Action<bool> OnPressStateChanged;
        public event Action<char, float> OnSymbolAdded;
        public event Action<float, char> OnHolding;

        private char _lastPreviewSymbol;
        private float _pressStartTime;
        private bool _isPressing;

        public float DotThreshold => _dotThreshold;

        void Update()
        {
            ProcessHolding(Time.time);
        }

        /// <remarks>
        /// Called when holding the flashlight.
        /// </remarks>
        public void OnButtonDown()
        {
            _isPressing = true;
            _pressStartTime = Time.time;
            _lastPreviewSymbol = '\0';

            OnPressStateChanged?.Invoke(true);
        }

        /// <remarks>
        /// Called when releasing finger from the flashlight.
        /// </remarks>
        public void OnButtonUp()
        {
            if (!_isPressing)
                return;

            float duration = Time.time - _pressStartTime;
            _isPressing = false;

            char finalSymbol = duration < _dotThreshold ? '.' : '-';

            OnPressStateChanged?.Invoke(false);
            OnSymbolAdded?.Invoke(finalSymbol, Time.time); // One single event for completion
        }

        private void ProcessHolding(float currentTime)
        {
            if (!_isPressing)
                return;

            float duration = currentTime - _pressStartTime;

            float progress = Mathf.Clamp01(duration / _dotThreshold);
            char previewSymbol = duration < _dotThreshold ? '.' : '-';

            OnHolding?.Invoke(progress, previewSymbol);

            if (previewSymbol != _lastPreviewSymbol)
                _lastPreviewSymbol = previewSymbol;
        }
    }
}
