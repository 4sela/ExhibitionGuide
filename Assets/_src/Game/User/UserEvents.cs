using System;

namespace Game.Systems.User
{
    public static class UserEvents
    {
        public static Action<string> SetName;
        public static Action<string> EnableStartButtonIfNameExists;

        public static Func<string> GetName;
    }
}
