using UnityEngine;

namespace Game.Configs
{
    public sealed class GlobalState : MonoBehaviour
    {
        private static bool _isAudioEnabled;

        void OnEnable()
        {
            GlobalStateEvents.SetDefaultAudioBehaviour += SetAudioEnabled;
            GlobalStateEvents.GetDefaultAudioBehaviour += GetAudioEnabled;
        }

        void OnDisable()
        {
            GlobalStateEvents.SetDefaultAudioBehaviour -= SetAudioEnabled;
            GlobalStateEvents.GetDefaultAudioBehaviour -= GetAudioEnabled;
        }

        private static void SetAudioEnabled(bool isEnabled)
        {
            _isAudioEnabled = isEnabled;
        }

        private static bool GetAudioEnabled() => _isAudioEnabled;
    }
}
