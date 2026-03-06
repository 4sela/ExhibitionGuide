using UnityEngine;
using TMPro;
using Game.Systems.Player;

namespace Game.UI.Modals
{
    public sealed class NameModal : MonoBehaviour
    {
        public TMP_InputField inputField;

        public void OnConfirm()
        {
            PlayerNameSystem.Instance.SetName(inputField.text);
            gameObject.SetActive(false);
        }
    }
}
