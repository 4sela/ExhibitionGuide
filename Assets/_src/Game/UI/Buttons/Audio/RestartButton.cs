using UnityEngine;
using UnityEngine.UI;
using Game.Systems.Haptics;

namespace Game.UI.Buttons.Audio
{
    public sealed class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button restartButton;

        void Start()
        {
            restartButton.onClick.AddListener(RestartAudio);
        }

        private void RestartAudio()
        {
            HapticsService.PlayClick();
            AudioManager.Instance.Voice.ResetVoice();
        }
    }
}
