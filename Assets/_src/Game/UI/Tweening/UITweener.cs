using UnityEngine;
using DG.Tweening;

namespace Game.UI.Tweening
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// Add as a component to every UI element that needs tweening.
    /// </remarks>
    public sealed class UITweener : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private Vector3 hiddenScale = new Vector3(0.9f, 0.9f, 1f);
        [SerializeField] private Vector3 shownScale = Vector3.one;
        [SerializeField] private CanvasGroup canvasGroup;

        void Awake()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Plays a 'Show' animation when called
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
        /// Plays a 'Hide' animation when called
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
