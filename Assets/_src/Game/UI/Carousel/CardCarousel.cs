using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game.UI.Carousel
{
    public sealed class CardCarousel : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("UI References")]
        public ScrollRect scrollRect;
        public RectTransform viewport;
        public RectTransform content;
        public RectTransform[] cards;

        [Header("Carousel Settings")]
        public float focusedScale = 1.1f;
        public float unfocusedScale = 0.8f;
        public float focusedAlpha = 1f;
        public float unfocusedAlpha = 0.5f;
        public float snapSpeed = 10f;
        public float focusRange = 500f;

        [Header("State (Visible for other scripts & Debugging)")]
        [SerializeField] private int currentIndex = 0;

        private bool isDragging = false;
        private CanvasGroup[] canvasGroups;
        private int closestCardIndex = 0;
        private float targetPositionX;

        void Start()
        {
            canvasGroups = new CanvasGroup[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                canvasGroups[i] = cards[i].GetComponent<CanvasGroup>();
            }

            Canvas.ForceUpdateCanvases();

            float trueViewportCenterX = viewport.TransformPoint(viewport.rect.center).x;

            float differenceToCenter = (trueViewportCenterX - cards[0].position.x) / content.lossyScale.x;
            targetPositionX = content.anchoredPosition.x + differenceToCenter;

            content.anchoredPosition = new Vector2(targetPositionX, content.anchoredPosition.y);
        }

        void Update()
        {
            UpdateCardVisuals();

            if (!isDragging)
            {
                Vector2 currentPos = content.anchoredPosition;
                currentPos.x = Mathf.Lerp(currentPos.x, targetPositionX, Time.deltaTime * snapSpeed);
                content.anchoredPosition = currentPos;
            }
        }

        private void UpdateCardVisuals()
        {
            float minDistance = float.MaxValue;

            float trueViewportCenterX = viewport.TransformPoint(viewport.rect.center).x;

            for (int i = 0; i < cards.Length; i++)
            {
                float distanceToCenter = Mathf.Abs(trueViewportCenterX - cards[i].position.x);

                if (distanceToCenter < minDistance)
                {
                    minDistance = distanceToCenter;
                    closestCardIndex = i;
                }

                float focusFactor = 1f - Mathf.Clamp01(distanceToCenter / focusRange);

                float targetScale = Mathf.Lerp(unfocusedScale, focusedScale, focusFactor);
                cards[i].localScale = new Vector3(targetScale, targetScale, 1f);

                if (canvasGroups[i] != null)
                {
                    canvasGroups[i].alpha = Mathf.Lerp(unfocusedAlpha, focusedAlpha, focusFactor);
                }
            }

            currentIndex = closestCardIndex;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            scrollRect.velocity = Vector2.zero;

            float trueViewportCenterX = viewport.TransformPoint(viewport.rect.center).x;

            float differenceToCenter = (trueViewportCenterX - cards[closestCardIndex].position.x) / content.lossyScale.x;
            targetPositionX = content.anchoredPosition.x + differenceToCenter;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public void OpenSelectedPanelFromCarousel()
        {
            switch (currentIndex)
            {
                case 0:
                    UIManager.Instance.ShowScreen(ScreenType.ContextSetup);
                    break;
                case 1:
                    UIManager.Instance.ShowScreen(ScreenType.NarrativeSetup);
                    break;
                case 2:
                    UIManager.Instance.ShowScreen(ScreenType.Gallery);
                    break;
            }
        }
    }
}
