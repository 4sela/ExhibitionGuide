using UnityEngine;
using UnityEngine.UI;
using Game.Systems.User;

namespace Game.UI.Screens.StartScreen
{
    /// <summary>
    /// Controls visibility of the Start Button in the Start Screen.
    /// </summary>
    public sealed class StartButtonController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        void Start()
        {
            startButton.interactable = false;
        }

        void OnEnable() => UserEvents.EnableStartButtonIfNameExists += EnableStartButtonIfNameExists;
        void OnDisable() => UserEvents.EnableStartButtonIfNameExists -= EnableStartButtonIfNameExists;

        private void EnableStartButtonIfNameExists(string name)
        {
            startButton.interactable = !string.IsNullOrEmpty(name);
        }
    }
}
