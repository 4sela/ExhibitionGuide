using UnityEngine;
using Game.User.Data;

namespace Game.User
{
    /// <summary>
    /// Wrapper class for UserData.
    /// </summary>
    [System.Obsolete]
    public sealed class UserProfile : MonoBehaviour
    {
        [SerializeField] private UserData _userData;

        void OnEnable()
        {
            Debug.LogError("Reminder to remove the User namespace!");
            UserEvents.SetUserData += SetUserData;
            UserEvents.GetUserData += GetUserData;
        }

        void OnDisable()
        {
            UserEvents.SetUserData -= SetUserData;
            UserEvents.GetUserData -= GetUserData;
        }

        private void SetUserData(UserData userData) => _userData = userData;
        private UserData GetUserData() => _userData;
    }
}
