using UnityEngine;
using TMPro;
using Game.Systems.Player;

namespace Game.UI.Screens.WelcomeScreen
{
    public sealed class WelcomeScreenController : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeNameText;

        void OnEnable()
        {
            welcomeNameText.text = $"Hej {PlayerEvents.GetName?.Invoke()}";
        }
    }
}
