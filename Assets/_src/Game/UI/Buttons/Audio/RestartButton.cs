using UnityEngine;
using UnityEngine.UI;

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
            AudioManager.Instance.Voice.ResetVoice();
        }
    }
}
