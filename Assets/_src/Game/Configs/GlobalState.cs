using UnityEngine;

namespace Game.Configs
{
    public static class GlobalState
    {
        public static bool IsAudioEnabled { get; private set; }

        public static Action<bool> SetDefaultAudioBehaviour;

        public static void SetAudioEnabled(bool isEnabled)
        {
            IsAudioEnabled = isEnabled;
            PlayerPrefs.SetInt("AudioEnabled", isEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
