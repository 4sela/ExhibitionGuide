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
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject welcomeScreen;
        [SerializeField] private GameObject exhibitionScreen;

        [Header("Modals")]
        [SerializeField] private GameObject nameInputModal;

        void Start()
        {
            // We disable all screens first to ensure the StartScreen is showed first
            startScreen.SetActive(false);
            welcomeScreen.SetActive(false);
            exhibitionScreen.SetActive(false);

            // Now we show StartScreen with animation
            startScreen.GetComponent<UITweener>().Show();
        }

        /// <summary>
        /// Master function to manage panels.
        /// </summary>
        private void ShowScreen(GameObject screenToShow)
        {
            // Hide everything EXCEPT the one we want to show
            if (startScreen != screenToShow) Hide(startScreen);
            if (welcomeScreen != screenToShow) Hide(welcomeScreen);
            if (exhibitionScreen != screenToShow) Hide(exhibitionScreen);

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
        public void OnStartButtonPressed() => ShowScreen(welcomeScreen);

        /// <summary>
        /// Transition to the Exhibition Screen.
        /// </summary>
        /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void OnExhibitionSelected() => ShowScreen(exhibitionScreen);

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public void OnNameButtonPressed() => nameInputModal.GetComponent<UITweener>().Show();

    }
}
