using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.User;
using Game.User.Data;
using Game.User.Data.Enums;

namespace Game.UI.Screens.StartScreen
{
    public sealed class StartScreenController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button maleButton;
        [SerializeField] private Button femaleButton;
        [SerializeField] private CanvasGroup maleCanvas;
        [SerializeField] private CanvasGroup femaleCanvas;

        [Header("Visuals")]
        [SerializeField] private float buttonAlpha = 0.4f;

        private UserGender selectedGender = UserGender.Undefined;

        void Start()
        {
            nameInputField.onEndEdit.AddListener(SaveName);

            // Instead of assigning listeners in the inspector, we assign them straight in code
            maleButton.onClick.AddListener(() => OnGenderClicked(UserGender.Male));
            femaleButton.onClick.AddListener(() => OnGenderClicked(UserGender.Female));

            UpdateGenderButtonVisuals();
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveName(string inputName)
        {
            // Gets userData, though if it doesn't exist it creates a new one for safety measures
            UserData data = UserEvents.GetUserData?.Invoke() ?? new UserData();
            data.name = inputName;

            UserEvents.SetUserData?.Invoke(data);
            UserEvents.OnUserDataUpdated?.Invoke(data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        /// Hooked to gender buttons
        /// </remarks>
        private void OnGenderClicked(UserGender gender)
        {
            selectedGender = (selectedGender == gender) ? UserGender.Undefined : gender;

            // Gets userData, though if it doesn't exist it creates a new one for safety measures
            UserData data = UserEvents.GetUserData?.Invoke() ?? new UserData();
            data.gender = selectedGender;

            UserEvents.SetUserData?.Invoke(data);
            UserEvents.OnUserDataUpdated?.Invoke(data);

            UpdateGenderButtonVisuals();
        }

        /// <summary>
        /// Updates the visuals of the gender buttons
        /// </summary>
        private void UpdateGenderButtonVisuals()
        {
            maleCanvas.alpha = (selectedGender == UserGender.Male) ? 1 : buttonAlpha;
            femaleCanvas.alpha = (selectedGender == UserGender.Female) ? 1 : buttonAlpha;
        }
    }
}
