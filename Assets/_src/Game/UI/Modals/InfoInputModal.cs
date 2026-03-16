using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.User;
using Game.User.Data;
using Game.User.Data.Enums;
using Game.UI.Tweening;

namespace Game.UI.Modals
{
    // TODO: Class name refactor to something like InfoInputModal
    public sealed class InfoInputModal : MonoBehaviour
    {
        [Header("Name")]
        [SerializeField] private TMP_InputField inputField;

        [Header("Gender")]
        [SerializeField] private Button maleButton;
        [SerializeField] private Button femaleButton;

        [SerializeField] private Image maleImage;
        [SerializeField] private Image femaleImage;

        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private Color activeColor = Color.white;

        private UserGender selectedGender = UserGender.Undefined;

        void Start()
        {
            // Initialize visuals
            SetVisuals();

            maleButton.onClick.AddListener(() => OnGenderClicked(UserGender.Male));
            femaleButton.onClick.AddListener(() => OnGenderClicked(UserGender.Female));
        }

        /// <summary>
        /// Sets the name to the content of the input field. Then enables Start button if name exists.
        /// </summary>
        /// <remarks>
        /// Hook to 'Bekræft' button in the NameInputModal game object.
        /// TODO: Needs to implement Gender too
        /// </remarks>
        public void OnConfirm()
        {
            UserData newUserData = new()
            {
                name = inputField.text,
                gender = selectedGender
            };

            UserEvents.SetUserData?.Invoke(newUserData);

            UserData userData = UserEvents.GetUserData.Invoke();
            UserEvents.EnableStartButtonIfNameExists?.Invoke(userData.name);

            GetComponent<UITweener>().Hide();
        }

        private void OnGenderClicked(UserGender gender)
        {
            if (selectedGender == gender)
            {
                selectedGender = UserGender.Undefined;
            }
            else
            {
                selectedGender = gender;
                UserData newUserData = new()
                {
                    gender = gender
                };

                UserEvents.SetUserData?.Invoke(newUserData);
            }

            SetVisuals();
        }

        private void SetVisuals()
        {
            maleImage.color = selectedGender == UserGender.Male ? activeColor : inactiveColor;
            femaleImage.color = selectedGender == UserGender.Female ? activeColor : inactiveColor;
        }

        public UserGender? GetSelectedGender() => selectedGender;
    }
}
