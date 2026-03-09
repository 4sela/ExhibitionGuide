using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// This class forces the phone to render the app in 120fps.
    /// </summary>
    /// <remarks>
    /// Instantiate once as a child in [SETTINGS] game object.
    /// </remarks>
    public sealed class AppInitialiser : MonoBehaviour
    {
        void Awake()
        {
            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 0;

            SetMobileFPS();
        }

        private void SetMobileFPS()
        {
            Screen.SetResolution(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                FullScreenMode.FullScreenWindow,
                new RefreshRate { numerator = 120, denominator = 1 }
            );
        }
    }
}
