using UnityEngine;

public sealed class AppInitialiser : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;

        Screen.SetResolution(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            FullScreenMode.FullScreenWindow,
            new RefreshRate { numerator = 120, denominator = 1 }
        );
    }
}
