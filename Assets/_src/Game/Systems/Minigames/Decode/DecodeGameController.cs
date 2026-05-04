using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Game.Systems.Minigames;
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
        [SerializeField] private Image locationFieldImage;
        [SerializeField] private Image messageFieldImage;
        [SerializeField] private GameObject selectorContainer;

        [Header("Feedback")]
        [SerializeField] private Color normalFieldColor = Color.white;
        [SerializeField] private Color wrongFieldColor = new Color(1f, 0.25f, 0.25f, 1f);
        [SerializeField] private float wrongFlashDuration = 0.2f;

        private int _locationIndex;
        private int _messageIndex;
        private Coroutine _wrongFlashRoutine;

        private void Awake()
        {
            AudioManager.Instance.Voice.StopVoice();
            FindManualUiReferences();
            UpdateSelectionTexts();
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
                return;
            }

            gameResult.IsCompleted = true;
            HideSelectorUi();
            gameResult.SummonResult();
        }

        // Kept for old prefab button hookups.
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
        }

        private void ChangeMessage(int direction)
        {
            _messageIndex = WrapIndex(_messageIndex + direction, messages.Length);
            UpdateSelectionTexts();
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
            if (_wrongFlashRoutine != null)
                StopCoroutine(_wrongFlashRoutine);

            _wrongFlashRoutine = StartCoroutine(WrongFlashRoutine());
        }

        private IEnumerator WrongFlashRoutine()
        {
            SetFieldColors(wrongFieldColor);
            yield return new WaitForSeconds(wrongFlashDuration);
            SetFieldColors(normalFieldColor);
            _wrongFlashRoutine = null;
        }

        private void SetFieldColors(Color color)
        {
            if (locationFieldImage != null)
                locationFieldImage.color = color;

            if (messageFieldImage != null)
                messageFieldImage.color = color;
        }

        private void FindManualUiReferences()
        {
            if (selectorContainer == null)
            {
                Transform selectors = FindChildRecursive(transform, "DecodeSelectors");
                if (selectors != null)
                    selectorContainer = selectors.gameObject;
            }

            if (locationText == null)
                locationText = FindText("TMP_LocationText", "LocationText", "LocationFieldText");

            if (messageText == null)
                messageText = FindText("TMP_MessageText", "MessageText", "MessageFieldText");

            if (locationFieldImage == null)
                locationFieldImage = FindImage("LocationField");

            if (messageFieldImage == null)
                messageFieldImage = FindImage("MessageField");

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

            if (locationFieldImage != null)
                locationFieldImage.transform.parent.gameObject.SetActive(false);

            if (messageFieldImage != null)
                messageFieldImage.transform.parent.gameObject.SetActive(false);
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
