using UnityEngine;
using System.Collections;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseBookShake : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float loopDelay = 3f;
        [SerializeField] private float jumpHeight = 15f;
        [SerializeField] private float scaleMultiplier = 1.05f;
        [SerializeField] private float jumpDuration = 0.4f;
        [SerializeField] private float shakeDuration = 0.5f;

        [Tooltip("How intense the shake is (in degrees).")]
        [SerializeField] private float shakeAngle = 8f;

        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private Vector3 _originalScale;
        private Quaternion _originalRotation;

        private Coroutine _loopCoroutine;
        private bool _hasSavedState = false;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        /// <remarks>
        /// Must be called by Tutorial Exit Button
        /// </remarks>
        public void StartAttentionLoop()
        {
            if (_rectTransform == null)
                return;

            if (!_hasSavedState)
            {
                _originalPosition = _rectTransform.anchoredPosition;
                _originalScale = _rectTransform.localScale;
                _originalRotation = _rectTransform.localRotation;
                _hasSavedState = true;
            }

            if (_loopCoroutine != null)
            {
                StopCoroutine(_loopCoroutine);
            }

            _loopCoroutine = StartCoroutine(LoopRoutine());
        }

        /// <remarks>
        /// Must be called by Morse Book Button
        /// </remarks>
        public void StopAttentionLoop()
        {
            if (_loopCoroutine != null)
            {
                StopCoroutine(_loopCoroutine);
                _loopCoroutine = null;
            }

            StopAllCoroutines();

            if (_hasSavedState)
            {
                ResetToOriginalState();
            }
        }

        /// <summary>
        /// Infinite loop that waits, animates, and repeats
        /// </summary>
        private IEnumerator LoopRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(loopDelay);
                yield return StartCoroutine(AttentionSequence());
            }
        }

        private IEnumerator AttentionSequence()
        {
            float halfJump = jumpDuration / 2f;
            Vector2 targetPosition = _originalPosition + new Vector2(0, jumpHeight);
            Vector3 targetScale = _originalScale * scaleMultiplier;

            float time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                _rectTransform.anchoredPosition = Vector2.Lerp(_originalPosition, targetPosition, calculatedTime);
                _rectTransform.localScale = Vector3.Lerp(_originalScale, targetScale, calculatedTime);
                yield return null;
            }

            float shakeTime = 0;
            int numberOfShakes = 4;
            while (shakeTime < shakeDuration)
            {
                shakeTime += Time.deltaTime;
                float zRotation = Mathf.Sin(shakeTime * Mathf.PI * 2 * numberOfShakes / shakeDuration) * shakeAngle;
                _rectTransform.localRotation = _originalRotation * Quaternion.Euler(0, 0, zRotation);
                yield return null;
            }
            _rectTransform.localRotation = _originalRotation;

            time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                _rectTransform.anchoredPosition = Vector2.Lerp(targetPosition, _originalPosition, calculatedTime);
                _rectTransform.localScale = Vector3.Lerp(targetScale, _originalScale, calculatedTime);
                yield return null;
            }

            ResetToOriginalState();
        }

        private void ResetToOriginalState()
        {
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.localScale = _originalScale;
            _rectTransform.localRotation = _originalRotation;
        }
    }
}
