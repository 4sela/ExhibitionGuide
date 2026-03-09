using UnityEngine;
using TMPro;
using Game.Systems.Player;
using Game.UI.Tweening;

namespace Game.UI.Modals
{
    public sealed class NameInputModal : MonoBehaviour
    {
        public TMP_InputField inputField;

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Hook to 'Bekræft' button in the NameInputModal game object.
        /// </remarks>
        public void OnConfirm()
        {
            PlayerEvents.SetName?.Invoke(inputField.text);
            GetComponent<UITweener>().Hide();
        }
    }
}
