using UnityEngine;
using System.Collections.Generic;
using Game.UI.Tweening;
using Game.Systems.Contexts;

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
        /// <summary>
        /// Unity doesn't serialise dictionaries so this is the best way I can come up with to associate screen panels with a label.
        /// This liberates us from remembering indices.
        /// </summary>
        [System.Serializable]
        private struct ScreenEntry
        {
            public ScreenType type;
            public GameObject screenObject;
        }

        [Header("Screens")]
        [SerializeField] private ScreenEntry[] allScreens;

        [Header("Modals")]
        [SerializeField] private GameObject nameInputModal;
        [SerializeField] private GameObject quitConfirmationModal;

        private Dictionary<ScreenType, GameObject> screenMap;

        void Awake()
        {
            screenMap = new Dictionary<ScreenType, GameObject>();

            for (int i = 0; i < allScreens.Length; i++)
            {
                ScreenEntry entry = allScreens[i];
                screenMap[entry.type] = entry.screenObject;
            }
        }

        void Start()
        {
            // We disable all screens first to ensure the StartScreen is showed first
            for (int i = 0; i < allScreens.Length; i++)
                allScreens[i].screenObject.SetActive(false);


            // Now we show StartScreen with animation
            ShowScreen(ScreenType.Start);
            ContextManager.Instance.CloseAllPanels();
        }

        /// <summary>
        /// Master function to manage panels.
        /// </summary>
        public void ShowScreen(ScreenType type)
        {
            // Hide everything EXCEPT the one we want to show
            for (int i = 0; i < allScreens.Length; i++)
            {
                ScreenEntry entry = allScreens[i];
                GameObject screen = entry.screenObject;

                // Show the target screen
                if (entry.type == type)
                {
                    UITweener tweener = screen.GetComponent<UITweener>();

                    if (tweener != null)
                        tweener.Show();

                    else
                        screen.SetActive(true);
                }

                else
                    Hide(screen);
            }
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
        ///
        /// </summary>
        /// <remarks> Button hookup.</remarks>
        public void OnNameButtonPressed() => nameInputModal.GetComponent<UITweener>().Show();

        /// <summary>
        /// Transition to the Welcome Screen.
        /// </summary>
        /// <remarks>
        /// Button hookup.
        /// </remarks>
        public void ToWelcomeScreen()
        {
            AudioManager.Instance.Voice.StopVoice();
            ShowScreen(ScreenType.Welcome);
        }
        public void ToContextSetupScreen()
        {
            AudioManager.Instance.Voice.StopVoice();
            ShowScreen(ScreenType.ContextSetup);
        }
        public void ToContextScreen() => ShowScreen(ScreenType.Context);
        public void ToNarrativeSetupScreen() => ShowScreen(ScreenType.NarrativeSetup);
        public void ToNarrativeScreen() => ShowScreen(ScreenType.Narrative);

    }
}
