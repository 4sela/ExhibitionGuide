using UnityEngine;
using System.Collections;

namespace Game.Systems.Minigames.Morse
{
    public sealed class MorseBookShake : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float jumpHeight = 15f;
        [SerializeField] private float scaleMultiplier = 1.05f;
        [SerializeField] private float jumpDuration = 0.4f;
        [SerializeField] private float shakeDuration = 0.5f;

        [Tooltip("How intense the shake is (in degrees).")]
        [SerializeField] private float shakeAngle = 8f;

        [SerializeField] private float loopDelay = 3f;

        private RectTransform rectTransform;
        private Vector2 originalPosition;
        private Vector3 originalScale;
        private Quaternion originalRotation;

        private Coroutine loopCoroutine;
        private bool hasSavedState = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        /// <remarks>
        /// Must be called by Tutorial Exit Button
        /// </remarks>
        public void StartAttentionLoop()
        {
            if (rectTransform == null)
                return;

            if (!hasSavedState)
            {
                originalPosition = rectTransform.anchoredPosition;
                originalScale = rectTransform.localScale;
                originalRotation = rectTransform.localRotation;
                hasSavedState = true;
            }

            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
            }

            loopCoroutine = StartCoroutine(LoopRoutine());
        }

        /// <remarks>
        /// Must be called by Morse Book Button
        /// </remarks>
        public void StopAttentionLoop()
        {
            if (loopCoroutine != null)
            {
                StopCoroutine(loopCoroutine);
                loopCoroutine = null;
            }

            StopAllCoroutines();

            if (hasSavedState)
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
            Vector2 targetPosition = originalPosition + new Vector2(0, jumpHeight);
            Vector3 targetScale = originalScale * scaleMultiplier;

            float time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPosition, calculatedTime);
                rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, calculatedTime);
                yield return null;
            }

            float shakeTime = 0;
            int numberOfShakes = 4;
            while (shakeTime < shakeDuration)
            {
                shakeTime += Time.deltaTime;
                float zRotation = Mathf.Sin(shakeTime * Mathf.PI * 2 * numberOfShakes / shakeDuration) * shakeAngle;
                rectTransform.localRotation = originalRotation * Quaternion.Euler(0, 0, zRotation);
                yield return null;
            }
            rectTransform.localRotation = originalRotation;

            time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                rectTransform.anchoredPosition = Vector2.Lerp(targetPosition, originalPosition, calculatedTime);
                rectTransform.localScale = Vector3.Lerp(targetScale, originalScale, calculatedTime);
                yield return null;
            }

            ResetToOriginalState();
        }

        private void ResetToOriginalState()
        {
            rectTransform.anchoredPosition = originalPosition;
            rectTransform.localScale = originalScale;
            rectTransform.localRotation = originalRotation;
        }
    }
}
