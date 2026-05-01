using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Buttons
{
    public sealed class ButtonHaptics : MonoBehaviour
    {
        [Header("Haptics Settings")]
        [SerializeField] private bool usePredefined = false;

        [Tooltip("0 = CLICK, 1 = DOUBLE_CLICK, 2 = TICK, 5 = HEAVY_CLICK")]
        [Range(0, 5)]
        [SerializeField] private int predefinedEffect = 0;

        [Range(5, 25)]
        [SerializeField] private int durationMs = 10;

        [Range(0, 255)]
        [SerializeField] private int amplitude = 120;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(PlayHaptic);
        }

        private void PlayHaptic()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator"))
            {
                if (vibrator == null)
                    return;

                bool hasVibrator = vibrator.Call<bool>("hasVibrator");

                if (!hasVibrator)
                    return;

                using (var vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                {
                    AndroidJavaObject effect;

                    if (usePredefined)
                    {
                        effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                            "createPredefined",
                            predefinedEffect
                        );
                    }
                    else
                    {
                        effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                            "createOneShot",
                            durationMs,
                            amplitude
                        );
                    }

                    vibrator.Call("vibrate", effect);
                }
            }
#endif
        }
    }
}
