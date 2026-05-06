using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Game.UI.Events;
using Game.Systems.Haptics;

namespace Game.Configs.Audio
{
    public sealed class AudioPrompt : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Enable")]
        [SerializeField] private Button enableAudioButton;
        [SerializeField] private CanvasGroup enableCanvas;
        [SerializeField] private Transform enableTransform;

        [Header("Disable")]
        [SerializeField] private Button disableAudioButton;
        [SerializeField] private CanvasGroup disableCanvas;
        [SerializeField] private Transform disableTransform;

        [Header("Visuals: State")]
        [SerializeField] private float inactiveAlpha = 0.4f;
        [SerializeField] private float inactiveScale = 0.85f;

        [Header("Visuals: Timings")]
        [SerializeField] private float alphaDuration = 0.2f;
        [SerializeField] private float scaleDuration = 0.45f;

        [Header("Visuals: Tactile Feel")]
        [SerializeField] private float pressScaleMultipler = 0.95f;
        [SerializeField] private AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Spring Physics")]
        [Range(0.1f, 1f)][SerializeField] private float springBounciness = 0.4f;

        private Coroutine enableTransition;
        private Coroutine disableTransition;
        private Transform currentlyPressedTransform;
        private bool hasMadeInitialSelection = false;

        void Start()
        {
            enableAudioButton.onClick.AddListener(EnableAudio);
            disableAudioButton.onClick.AddListener(DisableAudio);

            SetInstantVisuals(enableCanvas, enableTransform, false);
            SetInstantVisuals(disableCanvas, disableTransform, false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.pointerEnter == enableAudioButton.gameObject)
            {
                currentlyPressedTransform = enableTransform;
                AnimateSquish(enableTransform, true);
            }
            else if (eventData.pointerEnter == disableAudioButton.gameObject)
            {
                currentlyPressedTransform = disableTransform;
                AnimateSquish(disableTransform, true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (currentlyPressedTransform != null)
            {
                AnimateSquish(currentlyPressedTransform, false);
                currentlyPressedTransform = null;
            }
        }

        private void AnimateSquish(Transform target, bool isPressed)
        {
            StopAllCoroutines();

            bool isAudioEnabled = GlobalStateEvents.GetDefaultAudioBehaviour.Invoke();
            float baseScale = 1f;

            if (target == enableTransform)
                baseScale = isAudioEnabled ? 1f : inactiveScale;

            if (target == disableTransform)
                baseScale = !isAudioEnabled ? 1f : inactiveScale;

            float targetScale = isPressed ? (baseScale * pressScaleMultipler) : baseScale;

            StartCoroutine(QuickSquish(target, targetScale));
        }

        private IEnumerator QuickSquish(Transform target, float targetScale)
        {
            Vector3 startScale = target.localScale;
            Vector3 endScale = new Vector3(targetScale, targetScale, 1f);
            float timeElapsed = 0f;
            float speed = 0.1f;

            while (timeElapsed < speed)
            {
                timeElapsed += Time.deltaTime;
                float time = timeElapsed / speed;

                float smoothTransition = time * time * (3f - 2f * time);

                target.localScale = Vector3.LerpUnclamped(startScale, endScale, smoothTransition);
                yield return null;
            }

            target.localScale = endScale;

            if (currentlyPressedTransform == null) UpdateButtonVisuals();
        }

        private void EnableAudio()
        {
            HapticsService.PlayClick();

            if (hasMadeInitialSelection && GlobalStateEvents.GetDefaultAudioBehaviour.Invoke())
                return;

            hasMadeInitialSelection = true;
            GlobalStateEvents.SetDefaultAudioBehaviour.Invoke(true);
            UIEvents.OnUserDataUpdated.Invoke();
            UpdateButtonVisuals();
        }

        private void DisableAudio()
        {
            HapticsService.PlayClick();

            if (hasMadeInitialSelection && !GlobalStateEvents.GetDefaultAudioBehaviour.Invoke())
                return;

            hasMadeInitialSelection = true;
            GlobalStateEvents.SetDefaultAudioBehaviour.Invoke(false);
            UIEvents.OnUserDataUpdated.Invoke();
            UpdateButtonVisuals();
        }

        private void UpdateButtonVisuals()
        {
            bool isAudioEnabled = GlobalStateEvents.GetDefaultAudioBehaviour.Invoke();

            if (enableTransition != null)
                StopCoroutine(enableTransition);

            if (disableTransition != null)
                StopCoroutine(disableTransition);

            enableTransition = StartCoroutine(AnimateVisuals(enableCanvas, enableTransform, isAudioEnabled));
            disableTransition = StartCoroutine(AnimateVisuals(disableCanvas, disableTransform, !isAudioEnabled));
        }

        private void SetInstantVisuals(CanvasGroup canvas, Transform btnTransform, bool isActive)
        {
            canvas.alpha = isActive ? 1f : inactiveAlpha;
            float scale = isActive ? 1f : inactiveScale;
            btnTransform.localScale = new Vector3(scale, scale, 1f);
        }

        private IEnumerator AnimateVisuals(CanvasGroup canvasGroup, Transform buttonTransform, bool isActive)
        {
            float targetAlpha = isActive ? 1f : inactiveAlpha;
            float targetScale = isActive ? 1f : inactiveScale;

            float startAlpha = canvasGroup.alpha;
            Vector3 startScale = buttonTransform.localScale;
            Vector3 endScale = new Vector3(targetScale, targetScale, 1f);

            float timeElapsed = 0f;

            float maxDuration = Mathf.Max(alphaDuration, scaleDuration);

            while (timeElapsed < maxDuration)
            {
                timeElapsed += Time.deltaTime;

                float alphaTime = Mathf.Clamp01(timeElapsed / alphaDuration);
                float scaleTime = Mathf.Clamp01(timeElapsed / scaleDuration);

                float alphaTransition = alphaCurve.Evaluate(alphaTime);
                float scaleTransition = EvaluateSpring(scaleTime, springBounciness);

                canvasGroup.alpha = Mathf.LerpUnclamped(startAlpha, targetAlpha, alphaTransition);
                buttonTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, scaleTransition);

                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            buttonTransform.localScale = endScale;
        }

        private float EvaluateSpring(float time, float bounciness)
        {
            if (time == 0 || time == 1)
                return time;

            return Mathf.Pow(2, -10 * time) * Mathf.Sin((time - bounciness / 4f) * (Mathf.PI * 2f) / bounciness) + 1f;
        }
    }
}
