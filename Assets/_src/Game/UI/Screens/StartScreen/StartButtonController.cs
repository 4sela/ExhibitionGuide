using UnityEngine;
using UnityEngine.UI;
using Game.User;
using Game.User.Data;
using Game.User.Data.Enums;

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

        void OnEnable() => UserEvents.OnUserDataUpdated += ValidateUserData;
        void OnDisable() => UserEvents.OnUserDataUpdated -= ValidateUserData;

        private void ValidateUserData(UserData data)
        {
            bool hasName = !string.IsNullOrWhiteSpace(data.name);
            bool hasGender = data.gender != UserGender.Undefined;

            startButton.interactable = hasName && hasGender;
        }
    }
}
