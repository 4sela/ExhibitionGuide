using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems.Minigames.Tutorial
{
    public sealed class TutorialController : MonoBehaviour
    {
        [Header("Pages")]
        [SerializeField] private GameObject[] pages;

        [Header("Navigation Buttons")]
        [SerializeField] private Button backButton;
        [SerializeField] private Button forwardButton;
        [SerializeField] private Button exitButton;

        private int _currentPageIndex = 0;

        void Start()
        {
            CloseAllPages();
            InitialiseFirstPage();
        }

        private void CloseAllPages()
        {
            for (int i = 0; i < pages.Length; i++)
            {
                pages[i].SetActive(false);
            }
        }

        private void InitialiseFirstPage()
        {
            pages[0].SetActive(true);
            _currentPageIndex = 0;
            UpdateNavigationButtons();
        }

        public void ChangePages(int direction)
        {
            int newPageIndex = _currentPageIndex + direction;

            if (newPageIndex >= 0 && newPageIndex < pages.Length)
            {
                pages[_currentPageIndex].SetActive(false);
                pages[newPageIndex].SetActive(true);
                _currentPageIndex = newPageIndex;
                UpdateNavigationButtons();
            }
        }

        private void UpdateNavigationButtons()
        {
            if (pages[0].activeSelf)
            {
                backButton.interactable = false;
                forwardButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(false);
                return;
            }

            if (pages[^1].activeSelf)
            {
                backButton.interactable = true;
                forwardButton.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(true);
                return;
            }

            backButton.interactable = true;
            forwardButton.gameObject.SetActive(true);
            backButton.interactable = true;
            exitButton.gameObject.SetActive(false);
        }
    }
}
