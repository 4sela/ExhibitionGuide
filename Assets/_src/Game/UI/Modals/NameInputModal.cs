using UnityEngine;
using TMPro;
using Game.Systems.Player;
using Game.UI.Tweening;

namespace Game.UI.Modals
{
    public sealed class NameInputModal : MonoBehaviour
    {
        public TMP_InputField inputField;

        public void OnConfirm()
        {
            PlayerNameSystem.Instance.SetName(inputField.text);
            GetComponent<UITweener>().Hide();
        }
    }
}
