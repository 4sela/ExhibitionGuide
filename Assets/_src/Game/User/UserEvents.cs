using System;
using Game.User.Data;

namespace Game.User
{
    public unsafe static class UserEvents
    {
        public static Action<UserData> SetUserData;
        public static Action<string> EnableStartButtonIfNameExists;

        public static Func<UserData> GetUserData;
    }
}
