using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Buttons.Audio
{
    public sealed class MuteButtons : MonoBehaviour
    {
        [SerializeField] private Button muteButton;

        private bool _isMuted = false;

        void Start()
        {
            muteButton.onClick.AddListener(ToggleMute);
        }

        private void ToggleMute()
        {
            AudioManager.Instance.Voice.ToggleMute();
            _isMuted = !_isMuted;
        }
    }
}
