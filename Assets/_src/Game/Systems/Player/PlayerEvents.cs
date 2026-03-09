using System;

namespace Game.Systems.Player
{
    public static class PlayerEvents
    {
        public static Action<string> SetName;
        public static Action<string> OnNameChanged;

        public static Func<string> GetName;
    }
}
