using UnityEngine;
using TMPro;
using Game.User;

namespace Game.UI.Screens.WelcomeScreen
{
    /// <summary>
    /// Displays the name of the user in the Welcome Screen.
    /// </summary>
    public sealed class WelcomeScreenController : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeNameText;

        void OnEnable()
        {
            welcomeNameText.text = $"Hej {UserEvents.GetUserData.Invoke().name}";
        }
    }
}
