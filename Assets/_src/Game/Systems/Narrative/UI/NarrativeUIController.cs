using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Game.Systems.Narrative.Data;
using Game.Systems.Narrative.Runtime;
using DG.Tweening;

namespace Game.UI.Screens.Narrative
{
    public sealed class NarrativeUIController : MonoBehaviour
    {
        [Header("Text Elements")]
        [SerializeField] private TMP_Text bodyText;
        [SerializeField] private float textFadeDuration = 0.2f;

        [Header("Choices Setup")]
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private GameObject choiceButtonPrefab;
        [SerializeField] private Button defaultContinueButton;

        // Keep track of what we spawn so we can destroy it later
        private List<GameObject> spawnedChoices = new List<GameObject>();

        void OnEnable()
        {
            // Direct Singleton subscription (No NarrativeEvents needed)
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

        private void RenderNode(NarrativeNode node)
        {
            ClearChoices();

            // 1. Text Animation
            bodyText.DOFade(0f, textFadeDuration).OnComplete(() =>
            {
                bodyText.text = node.text;
                bodyText.DOFade(1f, textFadeDuration);
            });

            // 2. Spawn Choices
            int activeChoices = 0;

            foreach (var choice in node.choices)
            {
                // Optional: Skip choices if their condition isn't met
                if (!string.IsNullOrEmpty(choice.conditionKey) && !NarrativeManager.Instance.CheckCondition(choice.conditionKey))
                    continue;

                NarrativeChoice currentChoice = choice;

                GameObject newChoiceObj = Instantiate(choiceButtonPrefab, choicesContainer);
                newChoiceObj.SetActive(true);
                spawnedChoices.Add(newChoiceObj);

                // Setup the button logic
                // NOTE: Make sure your prefab has the ChoiceButtonController script attached!
                var btnCtrl = newChoiceObj.GetComponent<Game.UI.Narrative.UI.ChoiceButtonController>();
                btnCtrl.Setup(choice, () => NarrativeManager.Instance.Choose(choice));

                activeChoices++;
            }

            // 3. Handle Default Continue Button
            // If there are no choices, show the default button to proceed
            if (activeChoices == 0)
            {
                defaultContinueButton.gameObject.SetActive(true);
                defaultContinueButton.onClick.RemoveAllListeners();
                defaultContinueButton.onClick.AddListener(() => NarrativeManager.Instance.ContinueDefault());
            }

            // 4. Auto-Advance (if you set a timer in the node)
            if (node.autoAdvanceDelay > 0f)
            {
                DOVirtual.DelayedCall(node.autoAdvanceDelay, () => NarrativeManager.Instance.ContinueDefault());
            }
        }

        private void ClearChoices()
        {
            // Destroy all currently spawned buttons
            foreach (var choiceObj in spawnedChoices)
            {
                Destroy(choiceObj);
            }
            spawnedChoices.Clear();

            // Hide the default button just in case
            defaultContinueButton.gameObject.SetActive(false);
        }

        private void HideScreen()
        {
            ClearChoices();
            bodyText.text = string.Empty;
            gameObject.SetActive(false); // Hide this entire screen
        }
    }
}
