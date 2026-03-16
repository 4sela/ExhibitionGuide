using UnityEngine;

namespace Game.User
{
    /// <summary>
    ///
    /// </summary>
    [System.Obsolete]
    public sealed class UserNameSystem : MonoBehaviour
    {
        private string _userName;

        void OnEnable()
        {
            UserEvents.SetName += SetName;
            UserEvents.GetName += GetName;
        }

        void OnDisable()
        {
            UserEvents.SetName -= SetName;
            UserEvents.GetName -= GetName;
        }

        private void SetName(string newName) => _userName = newName;
        private string GetName() => _userName;
    }
}
