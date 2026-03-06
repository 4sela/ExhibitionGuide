using UnityEngine;
using UnityEngine.UI;
using Game.Systems.Player;

namespace Game.UI.Screens.StartScreen
{
    public sealed class StartButtonController : MonoBehaviour
    {
        public Button startButton;

        void Start()
        {
            startButton.interactable = false;

            PlayerEvents.OnNameChanged += (name) =>
            {
                startButton.interactable = !string.IsNullOrEmpty(name);
            };
        }
    }
}
