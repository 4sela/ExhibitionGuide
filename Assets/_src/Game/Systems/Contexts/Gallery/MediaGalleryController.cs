using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using Game.Systems.Haptics;

namespace Game.UI.Gallery
{
    public sealed class MediaGalleryController : MonoBehaviour
    {
        [Header("Preview")]
        [SerializeField] private GameObject previewPanel;
        [SerializeField] private RawImage imagePreview;
        [SerializeField] private RawImage videoPreview;
        [SerializeField] private VideoPlayer videoPlayer;

        [Header("Video")]
        [SerializeField] private RenderTexture videoRenderTexture;

        [Header("Swipe")]
        [SerializeField] private Texture[] imageSequence;
        [SerializeField] private float swipeThreshold = 80f;

        private RenderTexture _runtimeVideoTexture;
        private int _currentImageIndex = -1;

        private void Awake()
        {
            AttachSwipeForwarder(previewPanel);
            ClosePreview();
        }

        private void OnDestroy()
        {
            if (_runtimeVideoTexture != null)
            {
                _runtimeVideoTexture.Release();
                Destroy(_runtimeVideoTexture);
            }
        }

        public void OpenImage(Texture image)
        {
            if (image == null)
            {
                Debug.LogWarning("MediaGalleryController: Tried to open an image, but no Texture was assigned.");
                return;
            }

            HapticsService.PlayClick();
            _currentImageIndex = FindImageIndex(image);
            ShowImage(image);
        }

        public void OpenImage(RawImage sourceImage)
        {
            if (sourceImage == null)
            {
                Debug.LogWarning("MediaGalleryController: Tried to open an image, but no RawImage was assigned.");
                return;
            }

            OpenImage(sourceImage.texture);
        }

        public void ShowNextImage()
        {
            ShowImageFromCurrentOffset(1);
        }

        public void ShowPreviousImage()
        {
            ShowImageFromCurrentOffset(-1);
        }

        internal void HandlePreviewSwipe(float horizontalDelta)
        {
            if (Mathf.Abs(horizontalDelta) < swipeThreshold)
                return;

            if (horizontalDelta < 0f)
                ShowNextImage();
            else
                ShowPreviousImage();
        }

        private void ShowImage(Texture image)
        {
            StopVideo();

            if (previewPanel != null)
                previewPanel.SetActive(true);

            if (imagePreview != null)
            {
                imagePreview.gameObject.SetActive(true);
                imagePreview.texture = image;
                FitRawImageToTexture(imagePreview, image.width, image.height);
            }

            if (videoPreview != null)
                videoPreview.gameObject.SetActive(false);
        }

        private void ShowImageFromCurrentOffset(int offset)
        {
            if (imageSequence == null || imageSequence.Length <= 1)
                return;

            if (_currentImageIndex < 0)
                return;

            _currentImageIndex = WrapIndex(_currentImageIndex + offset, imageSequence.Length);
            Texture image = imageSequence[_currentImageIndex];
            if (image == null)
                return;

            HapticsService.PlayClick();
            ShowImage(image);
        }

        public void OpenVideo(VideoClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("MediaGalleryController: Tried to open a video, but no VideoClip was assigned.");
                return;
            }

            if (videoPlayer == null || videoPreview == null)
            {
                Debug.LogWarning("MediaGalleryController: VideoPlayer and VideoPreview must be assigned before opening video.");
                return;
            }

            HapticsService.PlayClick();
            _currentImageIndex = -1;

            if (previewPanel != null)
                previewPanel.SetActive(true);

            if (imagePreview != null)
                imagePreview.gameObject.SetActive(false);

            videoPreview.gameObject.SetActive(true);
            FitRawImageToTexture(videoPreview, Mathf.Max(1, (int)clip.width), Mathf.Max(1, (int)clip.height));
            videoPlayer.Stop();
            videoPlayer.clip = clip;
            videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            videoPlayer.targetTexture = GetVideoTexture();
            videoPreview.texture = videoPlayer.targetTexture;
            videoPlayer.Play();
        }

        public void ClosePreview()
        {
            HapticsService.PlayClick();
            StopVideo();

            if (previewPanel != null)
                previewPanel.SetActive(false);

            _currentImageIndex = -1;

            if (imagePreview != null)
                imagePreview.texture = null;

            if (videoPreview != null)
                videoPreview.texture = null;
        }

        private RenderTexture GetVideoTexture()
        {
            if (videoRenderTexture != null)
                return videoRenderTexture;

            if (_runtimeVideoTexture == null)
            {
                _runtimeVideoTexture = new RenderTexture(1280, 720, 0);
                _runtimeVideoTexture.name = "Runtime Gallery Video Texture";
            }

            return _runtimeVideoTexture;
        }

        private void FitRawImageToTexture(RawImage rawImage, int width, int height)
        {
            if (rawImage == null || width <= 0 || height <= 0)
                return;

            RectTransform rectTransform = rawImage.rectTransform;
            RectTransform parentTransform = rectTransform.parent as RectTransform;
            if (parentTransform == null)
                return;

            Rect parentRect = parentTransform.rect;
            float parentWidth = parentRect.width;
            float parentHeight = parentRect.height;
            if (parentWidth <= 0f || parentHeight <= 0f)
                return;

            float mediaAspect = (float)width / height;
            float parentAspect = parentWidth / parentHeight;

            Vector2 fittedSize;
            if (mediaAspect > parentAspect)
                fittedSize = new Vector2(parentWidth, parentWidth / mediaAspect);
            else
                fittedSize = new Vector2(parentHeight * mediaAspect, parentHeight);

            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = fittedSize;
        }

        private void StopVideo()
        {
            if (videoPlayer == null)
                return;

            if (videoPlayer.isPlaying)
                videoPlayer.Stop();

            videoPlayer.clip = null;
        }

        private int FindImageIndex(Texture image)
        {
            if (imageSequence == null)
                return -1;

            for (int i = 0; i < imageSequence.Length; i++)
            {
                if (imageSequence[i] == image)
                    return i;
            }

            return -1;
        }

        private int WrapIndex(int index, int length)
        {
            return (index % length + length) % length;
        }

        private void AttachSwipeForwarder(GameObject target)
        {
            if (target == null)
                return;

            GalleryPreviewSwipeForwarder forwarder = target.GetComponent<GalleryPreviewSwipeForwarder>();
            if (forwarder == null)
                forwarder = target.AddComponent<GalleryPreviewSwipeForwarder>();

            forwarder.Initialise(this);
        }
    }

    internal sealed class GalleryPreviewSwipeForwarder : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private MediaGalleryController _controller;
        private Vector2 _dragStartPosition;

        public void Initialise(MediaGalleryController controller)
        {
            _controller = controller;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dragStartPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_controller == null)
                return;

            float horizontalDelta = eventData.position.x - _dragStartPosition.x;
            _controller.HandlePreviewSwipe(horizontalDelta);
        }
    }
}
