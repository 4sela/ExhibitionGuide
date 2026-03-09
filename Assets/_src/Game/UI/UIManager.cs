using UnityEngine;
using Game.UI.Tweening;

namespace Game.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    public sealed class UIManager : MonoBehaviour
    {
        [Header("Screen Panels")]
        public GameObject StartScreen;
        public GameObject WelcomeScreen;
        public GameObject ExhibitionScreen;

        [Header("Modals")]
        public GameObject NameInputModal;

        void Start()
        {
            // We disable all screens first to ensure the StartScreen is showed first
            StartScreen.SetActive(false);
            WelcomeScreen.SetActive(false);
            ExhibitionScreen.SetActive(false);

            // Now we show StartScreen with animation
            StartScreen.GetComponent<UITweener>().Show();
        }

        /// <summary>
        /// Master function to manage panels.
        /// </summary>
        private void ShowScreen(GameObject screenToShow)
        {
            // Hide everything EXCEPT the one we want to show
            if (StartScreen != screenToShow) Hide(StartScreen);
            if (WelcomeScreen != screenToShow) Hide(WelcomeScreen);
            if (ExhibitionScreen != screenToShow) Hide(ExhibitionScreen);

            // Now show the target
            UITweener tweener = screenToShow.GetComponent<UITweener>();
            if (tweener != null)
                tweener.Show();
            else
                screenToShow.SetActive(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        private void Hide(GameObject panel)
        {
            UITweener tweener = panel.GetComponent<UITweener>();
            if (tweener != null)
                tweener.Hide();
            else
                panel.SetActive(false);
        }

        /// <summary>
        /// Transition to the Welcome Screen.
        /// </summary>
        /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void OnStartButtonPressed() => ShowScreen(WelcomeScreen);

        /// <summary>
        /// Transition to the Exhibition Screen.
        /// </summary>
        /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void OnExhibitionSelected() => ShowScreen(ExhibitionScreen);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public void OnNameButtonPressed() => NameInputModal.GetComponent<UITweener>().Show();

    }
}
