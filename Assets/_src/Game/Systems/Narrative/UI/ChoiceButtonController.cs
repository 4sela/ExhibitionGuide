using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Systems.Narrative.Data;

namespace Game.UI.Narrative.UI
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ChoiceButtonController : MonoBehaviour
    {
        public Button button;
        public TMP_Text label;

        private System.Action onClick;

        /// <summary>
        ///
        /// </summary>
        public void Setup(NarrativeChoice choice, System.Action callback)
        {
            label.text = choice.label;
            onClick = callback;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}
