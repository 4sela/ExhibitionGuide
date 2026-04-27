using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Buttons.Audio
{
    public sealed class PausePlayButton : MonoBehaviour
    {
        [SerializeField] private Button pausePlayButton;
        [SerializeField] private RawImage pauseImage;
        [SerializeField] private RawImage playImage;

        private AudioManager _audioManager;
        private VoiceService _voiceService;

        void Start()
        {
            _audioManager = AudioManager.Instance;
            _voiceService = _audioManager.Voice;
            pausePlayButton.onClick.AddListener(TogglePlayPause);
            UpdateButtonVisuals();
        }

        private void TogglePlayPause()
        {
            if (_voiceService.IsPlaying())
            {
                Debug.Log("If");
                _voiceService.PauseVoice();
            }
            else if (_voiceService.IsPaused())
            {
                Debug.Log("Else If");
                _voiceService.UnPause();
            }
            else
            {
                Debug.Log("Else");
                if (_audioManager.voiceSource.clip != null)
                {
                    Debug.Log("Nested If");
                    _voiceService.PlayVoice(_audioManager.voiceSource.clip);
                }
            }

            UpdateButtonVisuals();
        }

        private void UpdateButtonVisuals()
        {
            bool isPlaying = _voiceService.IsPlaying();
            pauseImage.enabled = isPlaying;
            playImage.enabled = !isPlaying;
        }
    }
}
