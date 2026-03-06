using UnityEngine;
using TMPro;
using Game.Systems.Player;

namespace Game.UI.Screens.WelcomeScreen
{
    public sealed class WelcomeScreenController : MonoBehaviour
    {
        public TMP_Text welcomeText;

        void OnEnable()
        {
            welcomeText.text = $"Hej {PlayerNameSystem.Instance.PlayerName}";
        }
    }

}
