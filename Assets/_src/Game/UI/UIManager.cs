using UnityEngine;

namespace Game.UI
{
    public sealed class UIManager : MonoBehaviour
    {
        [Header("Screen Panels")]
        public GameObject StartScreen;
        public GameObject WelcomeScreen;
        public GameObject ExhibitionScreen;

        void Start()
        {
            // Start Screen must be initialised in the beginning!
            ShowScreen(StartScreen);
        }

        /// <summary>
        /// Master function to manage panels.
        /// </summary>
        private void ShowScreen(GameObject screenToShow)
        {
            HideAllPanels();
            RequestToShow(screenToShow);
        }

        private void HideAllPanels()
        {
            StartScreen.SetActive(false);
            WelcomeScreen.SetActive(false);
            ExhibitionScreen.SetActive(false);
        }

        private void RequestToShow(GameObject screenToShow) => screenToShow.SetActive(true);

        /// <summary>
        /// Transition to the Welcome Screen.
        /// </summary>
        /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void OnStartButtonPressed()
        {
            ShowScreen(WelcomeScreen);
        }

        /// <summary>
        /// Transition to the Exhibition Screen.
        /// </summary>
        /// /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void OnExhibitionSelected()
        {
            ShowScreen(ExhibitionScreen);
        }
    }
}
