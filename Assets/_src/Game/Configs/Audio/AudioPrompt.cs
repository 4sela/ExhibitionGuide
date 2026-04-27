using UnityEngine;
using UnityEngine.UI;
using Game.UI.Events;

namespace Game.Configs.Audio
{
    public sealed class AudioPrompt : MonoBehaviour
    {
        [Header("Enable")]
        [SerializeField] private Button enableAudioButton;
        [SerializeField] private CanvasGroup enableCanvas;

        [Header("Disable")]
        [SerializeField] private Button disableAudioButton;
        [SerializeField] private CanvasGroup disableCanvas;

        [Header("Visuals")]
        [SerializeField] private float buttonAlpha = 0.4f;

        void Start()
        {
            enableAudioButton.onClick.AddListener(EnableAudio);
            disableAudioButton.onClick.AddListener(DisableAudio);

            enableCanvas.alpha = buttonAlpha;
            disableCanvas.alpha = buttonAlpha;
        }

        private void EnableAudio()
        {
            GlobalStateEvents.SetDefaultAudioBehaviour.Invoke(true);
            Debug.Log(GlobalStateEvents.GetDefaultAudioBehaviour.Invoke());
            UIEvents.OnUserDataUpdated.Invoke();
            UpdateButtonVisuals();
        }

        private void DisableAudio()
        {
            GlobalStateEvents.SetDefaultAudioBehaviour.Invoke(false);
            Debug.Log(GlobalStateEvents.GetDefaultAudioBehaviour.Invoke());
            UIEvents.OnUserDataUpdated.Invoke();
            UpdateButtonVisuals();
        }

        private void UpdateButtonVisuals()
        {
            bool isAudioEnabled = GlobalStateEvents.GetDefaultAudioBehaviour.Invoke();

            enableCanvas.alpha = (isAudioEnabled) ? 1 : buttonAlpha;
            disableCanvas.alpha = (!isAudioEnabled) ? 1 : buttonAlpha;
        }
    }
}
