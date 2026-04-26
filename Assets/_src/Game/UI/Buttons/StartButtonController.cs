using UnityEngine;
using UnityEngine.UI;
using Game.UI.Events;

namespace Game.UI.Screens.StartScreen
{
    /// <summary>
    /// Controls visibility of the Start Button in the Start Screen.
    /// </summary>
    public sealed class StartButtonController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        void Awake()
        {
            if (startButton == null) return;
            startButton.interactable = false;
        }

        void OnEnable() => UIEvents.OnUserDataUpdated += EnableStartButton;
        void OnDisable() => UIEvents.OnUserDataUpdated -= EnableStartButton;

        private void EnableStartButton()
        {
            startButton.interactable = true;
        }
    }
}
