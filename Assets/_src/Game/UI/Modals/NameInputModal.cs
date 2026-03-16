using UnityEngine;
using TMPro;
using Game.User;
using Game.UI.Tweening;

namespace Game.UI.Modals
{
    // TODO: Class name refactor
    public sealed class NameInputModal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        /// <summary>
        /// Sets the name to the content of the input field. Then enables Start button if name exists.
        /// </summary>
        /// <remarks>
        /// Hook to 'Bekræft' button in the NameInputModal game object.
        /// </remarks>
        public void OnConfirm()
        {
            UserEvents.SetName?.Invoke(inputField.text);

            string playerName = UserEvents.GetName?.Invoke();
            UserEvents.EnableStartButtonIfNameExists?.Invoke(playerName);
            GetComponent<UITweener>().Hide();
        }
    }
}
