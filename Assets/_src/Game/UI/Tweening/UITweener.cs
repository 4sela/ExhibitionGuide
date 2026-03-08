using UnityEngine;
using DG.Tweening;

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
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        ///
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);

            canvasGroup.alpha = 0f;
            transform.localScale = hiddenScale;

            canvasGroup.DOFade(1f, duration);
            transform.DOScale(shownScale, duration).SetEase(Ease.OutBack);
        }

        /// <summary>
        ///
        /// </summary>
        public void Hide()
        {
            canvasGroup.DOFade(0f, duration);
            transform.DOScale(hiddenScale, duration)
                .SetEase(Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}
