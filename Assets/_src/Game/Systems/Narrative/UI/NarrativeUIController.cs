using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Game.Systems.Narrative.Data;
using Game.Systems.Narrative.Runtime;
using DG.Tweening;

namespace Game.UI.Narrative.UI
{
    /// <summary>
    ///
    /// </summary>
    public sealed class NarrativeUIController : MonoBehaviour
    {
        [Header("UI")]
        public TMP_Text bodyText;
        public Transform choicesContainer;
        public GameObject choiceButtonPrefab; // prefab with ChoiceButtonController
        public Button defaultContinueButton;
        public float textFadeDuration = 0.2f;

        private List<GameObject> spawnedChoices = new List<GameObject>();

        void OnEnable()
        {
            NarrativeManager.Instance.OnNodeEntered += RenderNode;
            NarrativeManager.Instance.OnNarrativeEnded += OnNarrativeEnded;
        }

        void OnDisable()
        {
            if (NarrativeManager.Instance != null)
            {
                NarrativeManager.Instance.OnNodeEntered -= RenderNode;
                NarrativeManager.Instance.OnNarrativeEnded -= OnNarrativeEnded;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void RenderNode(NarrativeNode node)
        {
            ClearChoices();

            bodyText.DOFade(0f, textFadeDuration).OnComplete(() =>
            {
                bodyText.text = node.text;
                bodyText.DOFade(1f, textFadeDuration);
            });

            foreach (var choice in node.choices)
            {
                bool allowed = true;
                if (!string.IsNullOrEmpty(choice.conditionKey))
                    allowed = NarrativeManager.Instance.CheckCondition(choice.conditionKey);

                if (!allowed) continue;

                var go = Instantiate(choiceButtonPrefab, choicesContainer);
                var ctrl = go.GetComponent<ChoiceButtonController>();
                ctrl.Setup(choice, () => NarrativeManager.Instance.Choose(choice));
                spawnedChoices.Add(go);
            }

            defaultContinueButton.gameObject.SetActive(spawnedChoices.Count == 0);
            defaultContinueButton.onClick.RemoveAllListeners();
            defaultContinueButton.onClick.AddListener(() => NarrativeManager.Instance.ContinueDefault());

            if (node.autoAdvanceDelay > 0f)
                DOVirtual.DelayedCall(node.autoAdvanceDelay, () => NarrativeManager.Instance.ContinueDefault());
        }

        /// <summary>
        ///
        /// </summary>
        private void ClearChoices()
        {
            for (int i = 0; i < spawnedChoices.Count; i++)
                Destroy(spawnedChoices[i]);
            spawnedChoices.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        private void OnNarrativeEnded()
        {
            ClearChoices();
            defaultContinueButton.gameObject.SetActive(false);
            bodyText.text = string.Empty;
        }
    }
}
