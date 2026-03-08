using UnityEngine;

namespace Game.UI.Tweening
{
    public sealed class UITweener : MonoBehaviour
    {
        [Header("Animation Settings")]
        public float duration = 0.25f;
        public Vector3 hiddenScale = new Vector3(0.9f, 0.9f, 1f);
        public Vector3 shownScale = Vector3.one;
        public CanvasGroup canvasGroup;

        void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        ///
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);

            canvasGroup.alpha = 0f;
            transform.localScale = hiddenScale;

            // LeanTween.alphaCanvas(canvasGroup, 1f, duration);
            //LeanTween.scale(gameObject, shownScale, duration).setEaseOutBack();
        }

        /// <summary>
        ///
        /// </summary>
        public void Hide()
        {/*
            LeanTween.alphaCanvas(canvasGroup, 0f, duration);
            LeanTween.scale(gameObject, hiddenScale, duration)
                .setEaseInBack()
                .setOnComplete(() => gameObject.SetActive(false));
                */
        }
    }
}
