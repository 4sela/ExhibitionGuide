using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using Game.Systems.Narrative.Data;
using Game.Systems.Narrative.Events;
using Game.Systems.Narrative.Runtime;
using Game.UI.Narrative.UI;

namespace Game.UI.Screens.Narrative
{
    public sealed class NarrativeUIController : MonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private float textFadeDuration = 0.2f;
        [SerializeField] private float timePerChar = 0.03f;

        [Header("Choices Setup")]
        [SerializeField] private GameObject choiceButtonPrefab;

        [Header("Containers")]
        [SerializeField] private Transform minigameContainer;
        [SerializeField] private Transform choicesContainerTransform;
        [SerializeField] private CanvasGroup choiceContainerCanvasGroup;

        [Header("Buttons")]
        [SerializeField] private Button defaultContinueButton;
        [SerializeField] private Button startMinigameButton;
        [SerializeField] private float buttonFadeDuration = 0.3f;

        private Button[] _buttonArray => new[]
        {
            defaultContinueButton,
            startMinigameButton
        };

        private List<GameObject> spawnedChoices = new List<GameObject>();
        private Coroutine typingCoroutine;

        void OnEnable()
        {
            if (NarrativeManager.Instance != null)
            {
                NarrativeManager.Instance.OnNodeEntered += RenderNode;
                NarrativeManager.Instance.OnNarrativeEnded += HideScreen;

                NarrativeManager.Instance.StartNarrative();
            }
        }

        void OnDisable()
        {
            if (NarrativeManager.Instance != null)
            {
                NarrativeManager.Instance.OnNodeEntered -= RenderNode;
                NarrativeManager.Instance.OnNarrativeEnded -= HideScreen;
            }
        }

        private void ResetChoiceContainerAlpha() => choiceContainerCanvasGroup.alpha = 0;

        private void RenderNode(NarrativeNode node)
        {
            ClearChoices();
#if !UNITY_EDITOR
            ResetChoiceContainerAlpha();
#endif
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            bodyText.DOFade(0f, textFadeDuration).OnComplete(() =>
            {
                Color c = bodyText.color;
                c.a = 1f;
                bodyText.color = c;

                typingCoroutine = StartCoroutine(TypeTextRoutine(node.text));
            });

            if (node.minigamePrefab != null)
            {
                SetupMinigamePrompt(node);
            }
            else
            {
                ShowChoices(node);
            }
        }

        private void SetupMinigamePrompt(NarrativeNode node)
        {
            startMinigameButton.gameObject.SetActive(true);

            startMinigameButton.onClick.RemoveAllListeners();
            startMinigameButton.onClick.AddListener(() =>
            {
                Instantiate(node.minigamePrefab, minigameContainer);

                startMinigameButton.gameObject.SetActive(false);
            });
        }

        private void ClearChoices()
        {
            for (int i = 0; i < spawnedChoices.Count; i++)
            {
                GameObject choiceObj = spawnedChoices[i];

                Destroy(choiceObj);
            }

            spawnedChoices.Clear();

            defaultContinueButton.gameObject.SetActive(false);
        }

        private void ShowChoices(NarrativeNode node)
        {
            int activeChoices = 0;

            for (int i = 0; i < node.choices.Count; i++)
            {
                NarrativeChoice choice = node.choices[i];

                NarrativeChoice currentChoice = choice;

                GameObject newChoiceObj = Instantiate(choiceButtonPrefab, choicesContainerTransform);
                newChoiceObj.SetActive(true);
                spawnedChoices.Add(newChoiceObj);

                // NOTE: Make sure your prefab has the ChoiceButtonController script attached!
                ChoiceButtonController btnCtrl = newChoiceObj.GetComponent<Game.UI.Narrative.UI.ChoiceButtonController>();
                btnCtrl.Setup(choice, () => NarrativeManager.Instance.Choose(choice));

                activeChoices++;
            }

            // If there are no choices, show the default button to proceed
            if (activeChoices == 0)
            {
                defaultContinueButton.gameObject.SetActive(true);
                defaultContinueButton.onClick.RemoveAllListeners();
                defaultContinueButton.onClick.AddListener(() => NarrativeManager.Instance.ContinueDefault());
            }
        }

        private void HideScreen()
        {
            ClearChoices();
            bodyText.text = string.Empty;
            gameObject.SetActive(false);
        }

        private IEnumerator TypeTextRoutine(string textToType)
        {

#if !UNITY_EDITOR
            for (int i = 0; i < _buttonArray.Length; i++)
                _buttonArray[i].interactable = false;
#endif

            bodyText.text = textToType;
            bodyText.maxVisibleCharacters = 0;

            for (int i = 0; i <= textToType.Length; i++)
            {
                bodyText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(timePerChar);
            }

            StartCoroutine(FadeInCanvasGroup(choiceContainerCanvasGroup, buttonFadeDuration));

            for (int i = 0; i < _buttonArray.Length; i++)
                _buttonArray[i].interactable = true;
        }

        private IEnumerator FadeInCanvasGroup(CanvasGroup canvasGroup, float duration)
        {
            canvasGroup.alpha = 0f;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 1f;
        }
    }
}
