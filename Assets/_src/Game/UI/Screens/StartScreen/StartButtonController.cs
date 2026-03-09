using UnityEngine;
using UnityEngine.UI;
using Game.Systems.Player;

namespace Game.UI.Screens.StartScreen
{
    public sealed class StartButtonController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        void Start()
        {
            startButton.interactable = false;
        }

        void OnEnable() => PlayerEvents.EnableStartButtonIfNameExists += EnableStartButtonIfNameExists;
        void OnDisable() => PlayerEvents.EnableStartButtonIfNameExists -= EnableStartButtonIfNameExists;

        private void EnableStartButtonIfNameExists(string name)
        {
            startButton.interactable = !string.IsNullOrEmpty(name);
        }
    }
}
