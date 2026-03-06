using UnityEngine;
using TMPro;
using Game.Systems.Player;

namespace Game.UI.Screens.WelcomeScreen
{
    public sealed class WelcomeScreenController : MonoBehaviour
    {
        public TMP_Text welcomeNameText;

        void OnEnable()
        {
            welcomeNameText.text = $"Hej {PlayerNameSystem.Instance.PlayerName}";
        }
    }

}
