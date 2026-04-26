using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Buttons.Audio
{
    public sealed class PausePlayButton : MonoBehaviour
    {
        [SerializeField] private Button pausePlayButton;
        [SerializeField] private RawImage pauseImage;
        [SerializeField] private RawImage playImage;

        private bool _isPlaying = false;

        void Start()
        {
            pausePlayButton.onClick.AddListener(TogglePlayPause);
            CheckConditions();
        }

        private void TogglePlayPause()
        {
            CheckConditions();
            _isPlaying = !_isPlaying;
        }

        private void CheckConditions()
        {
            if (_isPlaying)
            {
                AudioManager.Instance.Voice.PauseVoice();
                pauseImage.enabled = false;
                playImage.enabled = true;
            }
            else
            {
                AudioManager.Instance.Voice.UnPause();
                pauseImage.enabled = true;
                playImage.enabled = false;
            }
        }
    }
}
