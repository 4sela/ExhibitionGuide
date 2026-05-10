using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Diagnostics;
using Game.Systems.Haptics;

namespace Game.UI.Carousel
{
    public sealed class CardCarousel : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("UI References")]
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform viewport;
        [SerializeField] private RectTransform content;
        [SerializeField] private RectTransform[] cards;
        [SerializeField] private TextMeshProUGUI buttonText;

        [Header("Carousel Settings")]
        [SerializeField] private float focusedScale = 1.1f;
        [SerializeField] private float unfocusedScale = 0.8f;
        [SerializeField] private float focusedAlpha = 1f;
        [SerializeField] private float unfocusedAlpha = 0.5f;
        [SerializeField] private float focusRange = 500f;
        [SerializeField] private float smoothTime = 0.1f;

        private CanvasGroup[] _canvasGroups;
        private float _targetPositionX;
        private float _snapVelocity;
        private int _currentIndex = 0;
        private int _closestCardIndex = 0;
        private bool _isDragging = false;

        void Start()
        {
            _canvasGroups = new CanvasGroup[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                _canvasGroups[i] = cards[i].GetComponent<CanvasGroup>();
            }

            Canvas.ForceUpdateCanvases();

            float trueViewportCenterX = viewport.TransformPoint(viewport.rect.center).x;

            float differenceToCenter = (trueViewportCenterX - cards[0].position.x) / content.lossyScale.x;
            _targetPositionX = content.anchoredPosition.x + differenceToCenter;

            content.anchoredPosition = new Vector2(_targetPositionX, content.anchoredPosition.y);
        }

        void Update()
        {
            buttonText.text = UpdateButtonText();
            UpdateCardVisuals();

            if (!_isDragging)
            {
                Vector2 currentPos = content.anchoredPosition;
                currentPos.x = Mathf.SmoothDamp(currentPos.x, _targetPositionX, ref _snapVelocity, smoothTime);
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
                    _closestCardIndex = i;
                }

                float focusFactor = 1f - Mathf.Clamp01(distanceToCenter / focusRange);

                float targetScale = Mathf.Lerp(unfocusedScale, focusedScale, focusFactor);
                cards[i].localScale = new Vector3(targetScale, targetScale, 1f);

                if (_canvasGroups[i] != null)
                {
                    _canvasGroups[i].alpha = Mathf.Lerp(unfocusedAlpha, focusedAlpha, focusFactor);
                }
            }

            if (_currentIndex != _closestCardIndex)
            {
                _currentIndex = _closestCardIndex;
                HapticsService.PlayTick();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragging = false;
            scrollRect.velocity = Vector2.zero;

            float trueViewportCenterX = viewport.TransformPoint(viewport.rect.center).x;

            float differenceToCenter = (trueViewportCenterX - cards[_closestCardIndex].position.x) / content.lossyScale.x;
            _targetPositionX = content.anchoredPosition.x + differenceToCenter;
        }

        public int GetCurrentIndex()
        {
            return _currentIndex;
        }

        public void OpenSelectedPanelFromCarousel()
        {
            HapticsService.PlaySuccess();

            switch (_currentIndex)
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

        private string UpdateButtonText()
        {
            return _currentIndex switch
            {
                0 => "Udforsk nu",
                1 => "Start spillet",
                2 => "Åben galleriet"
            };
        }
    }
}
