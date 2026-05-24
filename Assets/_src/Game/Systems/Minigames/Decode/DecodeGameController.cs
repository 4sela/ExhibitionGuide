using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Game.Systems.Haptics;
using Game.Systems.Minigames;
using Game.Systems.Minigames.Morse;
using Game.Systems.Narrative.Runtime;

namespace Systems.Minigames.Decode
{
    public sealed class DecodeGameController : MonoBehaviour, IMinigame
    {
        [Header("Result")]
        [SerializeField] private GameResult gameResult;

        [Header("Solution")]
        [SerializeField] private string correctLocation = "Enebærodde";
        [SerializeField] private string correctMessage = "Nedkastning i nat";

        [Header("Options")]
        [SerializeField]
        private string[] locations =
        {
            "Enebærodde",
            "Hasmark",
            "Lumby",
            "Odense Havn"
        };

        [SerializeField]
        private string[] messages =
        {
            "Aflyst",
            "Nedkastning i nat",
            "Udskudt til i morgen nat",
            "Fare / tyskerne tæt på"
        };

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI locationText;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private GameObject selectorContainer;
        [SerializeField] private RawImage confirmButtonImage;
        [SerializeField] private MorseBookShake bookAttention;

        [Header("Feedback & Animation")]
        [SerializeField] private Color normalFieldColor = Color.white;
        [SerializeField] private Color wrongFieldColor = new Color(1f, 0.25f, 0.25f, 1f);

        [SerializeField] private float jumpHeight = 15f;
        [SerializeField] private float scaleMultiplier = 1.05f;
        [SerializeField] private float jumpDuration = 0.4f;
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private float shakeAngle = 8f;

        private int _locationIndex;
        private int _messageIndex;
        private Coroutine _wrongFlashRoutine;

        private Vector2 _originalButtonPos;
        private Vector3 _originalButtonScale;
        private Quaternion _originalButtonRot;

        private void Awake()
        {
            AudioManager.Instance.Voice.StopVoice();
            FindManualUiReferences();
            UpdateSelectionTexts();

            if (confirmButtonImage != null)
            {
                RectTransform rect = confirmButtonImage.rectTransform;
                _originalButtonPos = rect.anchoredPosition;
                _originalButtonScale = rect.localScale;
                _originalButtonRot = rect.localRotation;
            }
        }

        private void Start()
        {
            bookAttention?.StartAttentionLoop();
        }

        public void PreviousLocation() => ChangeLocation(-1);
        public void NextLocation() => ChangeLocation(1);
        public void PreviousMessage() => ChangeMessage(-1);
        public void NextMessage() => ChangeMessage(1);

        public void CheckAnswer()
        {
            if (!HasValidOptions())
                return;

            bool isCorrect =
                CurrentLocation == correctLocation &&
                CurrentMessage == correctMessage;

            if (!isCorrect)
            {
                FlashWrongSelection();
                HapticsService.PlayError();
                return;
            }

            gameResult.IsCompleted = true;
            HideSelectorUi();
            gameResult.SummonResult();
            HapticsService.PlaySuccess();
        }

        public void ButtonPress(bool arg)
        {
            gameResult.IsCompleted = arg;
            gameResult.SummonResult();
        }

        public void ProceedToNarrativeScreen()
        {
            NarrativeManager.Instance.ContinueDefault();
        }

        private string CurrentLocation => locations[_locationIndex];
        private string CurrentMessage => messages[_messageIndex];

        private void ChangeLocation(int direction)
        {
            _locationIndex = WrapIndex(_locationIndex + direction, locations.Length);
            UpdateSelectionTexts();
            PlaySelectionHaptic();
        }

        private void ChangeMessage(int direction)
        {
            _messageIndex = WrapIndex(_messageIndex + direction, messages.Length);
            UpdateSelectionTexts();
            PlaySelectionHaptic();
        }

        private int WrapIndex(int index, int length)
        {
            if (length <= 0)
                return 0;

            return (index % length + length) % length;
        }

        private void UpdateSelectionTexts()
        {
            if (locationText == null || messageText == null)
            {
                Debug.LogWarning("DecodeGameController: locationText and messageText must be assigned.");
                return;
            }

            if (locations == null || locations.Length == 0 || messages == null || messages.Length == 0)
            {
                Debug.LogWarning("DecodeGameController: locations and messages must contain at least one value.");
                return;
            }

            _locationIndex = WrapIndex(_locationIndex, locations.Length);
            _messageIndex = WrapIndex(_messageIndex, messages.Length);

            locationText.text = CurrentLocation;
            messageText.text = CurrentMessage;
        }

        private void FlashWrongSelection()
        {
            if (confirmButtonImage == null) return;

            if (_wrongFlashRoutine != null)
            {
                StopCoroutine(_wrongFlashRoutine);
                ResetButtonState();
            }

            _wrongFlashRoutine = StartCoroutine(WrongFlashRoutine());
        }

        private IEnumerator WrongFlashRoutine()
        {
            RectTransform rect = confirmButtonImage.rectTransform;
            float halfJump = jumpDuration / 2f;
            Vector2 targetPosition = _originalButtonPos + new Vector2(0, jumpHeight);
            Vector3 targetScale = _originalButtonScale * scaleMultiplier;

            float time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                rect.anchoredPosition = Vector2.Lerp(_originalButtonPos, targetPosition, calculatedTime);
                rect.localScale = Vector3.Lerp(_originalButtonScale, targetScale, calculatedTime);
                confirmButtonImage.color = Color.Lerp(normalFieldColor, wrongFieldColor, calculatedTime);
                yield return null;
            }

            float shakeTime = 0;
            int numberOfShakes = 4;
            while (shakeTime < shakeDuration)
            {
                shakeTime += Time.deltaTime;
                float zRotation = Mathf.Sin(shakeTime * Mathf.PI * 2 * numberOfShakes / shakeDuration) * shakeAngle;
                rect.localRotation = _originalButtonRot * Quaternion.Euler(0, 0, zRotation);
                yield return null;
            }
            rect.localRotation = _originalButtonRot;

            time = 0;
            while (time < halfJump)
            {
                time += Time.deltaTime;
                float calculatedTime = Mathf.SmoothStep(0, 1, time / halfJump);

                rect.anchoredPosition = Vector2.Lerp(targetPosition, _originalButtonPos, calculatedTime);
                rect.localScale = Vector3.Lerp(targetScale, _originalButtonScale, calculatedTime);
                confirmButtonImage.color = Color.Lerp(wrongFieldColor, normalFieldColor, calculatedTime);
                yield return null;
            }

            ResetButtonState();
            _wrongFlashRoutine = null;
        }

        private void ResetButtonState()
        {
            if (confirmButtonImage == null)
                return;

            confirmButtonImage.rectTransform.anchoredPosition = _originalButtonPos;
            confirmButtonImage.rectTransform.localScale = _originalButtonScale;
            confirmButtonImage.rectTransform.localRotation = _originalButtonRot;
            confirmButtonImage.color = normalFieldColor;
        }

        private void PlaySelectionHaptic()
        {
            HapticsService.PlayClick();
        }

        private void FindManualUiReferences()
        {
            if (selectorContainer == null)
            {
                Transform selectors = FindChildRecursive(transform, "CodeAssembler");
                if (selectors != null)
                    selectorContainer = selectors.gameObject;
            }

            locationText ??= FindText("TMP_LocationText", "LocationText", "LocationFieldText");
            messageText ??= FindText("TMP_MessageText", "MessageText", "MessageFieldText");

            if (bookAttention == null)
            {
                Transform book = FindChildRecursive(transform, "Book");
                if (book != null)
                    bookAttention = book.GetComponent<MorseBookShake>();
            }
        }

        private TextMeshProUGUI FindText(params string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                Transform child = FindChildRecursive(transform, names[i]);
                if (child != null && child.TryGetComponent(out TextMeshProUGUI text))
                    return text;
            }
            return null;
        }

        private Image FindImage(params string[] names)
        {
            for (int i = 0; i < names.Length; i++)
            {
                Transform child = FindChildRecursive(transform, names[i]);
                if (child != null && child.TryGetComponent(out Image image))
                    return image;
            }
            return null;
        }

        private Transform FindChildRecursive(Transform root, string childName)
        {
            if (root.name == childName)
                return root;

            for (int i = 0; i < root.childCount; i++)
            {
                Transform match = FindChildRecursive(root.GetChild(i), childName);
                if (match != null)
                    return match;
            }
            return null;
        }

        private void HideSelectorUi()
        {
            if (selectorContainer != null)
            {
                selectorContainer.SetActive(false);
                return;
            }
        }

        private bool HasValidOptions()
        {
            if (locations != null && locations.Length > 0 && messages != null && messages.Length > 0)
                return true;

            Debug.LogWarning("DecodeGameController: locations and messages must contain at least one value.");
            return false;
        }
    }
}
