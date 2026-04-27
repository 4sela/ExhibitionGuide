using UnityEngine;
using System;

namespace Game.Configs
{
    public static class GlobalStateEvents
    {
        public static Action<bool> SetDefaultAudioBehaviour;
        public static Func<bool> GetDefaultAudioBehaviour;
    }
}
