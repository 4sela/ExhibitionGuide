using UnityEngine;

namespace Game.Systems.Haptics
{
    public static class HapticsService
    {
        private const int HapticVirtualKey = 1;
        private const int HapticKeyboardTap = 3;
        private const int HapticContextClick = 6;
        private const int HapticConfirm = 16;
        private const int HapticReject = 17;

        private const int AndroidQ = 29;
        private const int AndroidS = 31;
        private const int DefaultAmplitude = -1;
        private const float MinimumInterval = 0.025f;

        private static float _lastPlayTime = -1f;

        public static void Play(HapticEffectType effectType)
        {
            if (Time.realtimeSinceStartup - _lastPlayTime < MinimumInterval)
                return;

            _lastPlayTime = Time.realtimeSinceStartup;

#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                PlayAndroid(effectType);
            }
            catch (AndroidJavaException exception)
            {
                Debug.LogWarning($"HapticsService: Android haptic failed: {exception.Message}");
            }
#endif
        }

        public static void PlayTick() => Play(HapticEffectType.Tick);
        public static void PlayClick() => Play(HapticEffectType.Click);
        public static void PlayHeavyClick() => Play(HapticEffectType.HeavyClick);
        public static void PlayDoubleClick() => Play(HapticEffectType.DoubleClick);
        public static void PlaySuccess() => Play(HapticEffectType.Success);
        public static void PlayError() => Play(HapticEffectType.Error);

#if UNITY_ANDROID && !UNITY_EDITOR
        private static void PlayAndroid(HapticEffectType effectType)
        {
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                if (activity == null)
                    return;

                if (PerformViewHaptic(activity, effectType))
                    return;

                PlayVibrationFallback(activity, effectType);
            }
        }

        private static bool PerformViewHaptic(AndroidJavaObject activity, HapticEffectType effectType)
        {
            using (AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow"))
            using (AndroidJavaObject decorView = window?.Call<AndroidJavaObject>("getDecorView"))
            {
                if (decorView == null)
                    return false;

                return decorView.Call<bool>("performHapticFeedback", ToHapticFeedbackConstant(effectType));
            }
        }

        private static int GetAndroidSdkVersion()
        {
            using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
                return version.GetStatic<int>("SDK_INT");
        }

        private static int ToHapticFeedbackConstant(HapticEffectType effectType)
        {
            switch (effectType)
            {
                case HapticEffectType.Tick:
                    return HapticKeyboardTap;
                case HapticEffectType.Click:
                    return HapticVirtualKey;
                case HapticEffectType.HeavyClick:
                    return HapticContextClick;
                case HapticEffectType.Success:
                    return HapticConfirm;
                case HapticEffectType.Error:
                case HapticEffectType.DoubleClick:
                    return HapticReject;
                default:
                    return HapticVirtualKey;
            }
        }

        private static void PlayVibrationFallback(AndroidJavaObject activity, HapticEffectType effectType)
        {
            int sdkVersion = GetAndroidSdkVersion();
            using (AndroidJavaObject vibrator = GetVibrator(activity, sdkVersion))
            {
                if (vibrator == null || !vibrator.Call<bool>("hasVibrator"))
                    return;

                PlayOneShot(vibrator, effectType);
            }
        }

        private static AndroidJavaObject GetVibrator(AndroidJavaObject activity, int sdkVersion)
        {
            if (sdkVersion >= AndroidS)
            {
                using (AndroidJavaObject vibratorManager = activity.Call<AndroidJavaObject>("getSystemService", "vibrator_manager"))
                {
                    if (vibratorManager != null)
                        return vibratorManager.Call<AndroidJavaObject>("getDefaultVibrator");
                }
            }

            return activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }

        private static void PlayOneShot(AndroidJavaObject vibrator, HapticEffectType effectType)
        {
            int durationMs;
            int amplitude;

            switch (effectType)
            {
                case HapticEffectType.Tick:
                    durationMs = 12;
                    amplitude = DefaultAmplitude;
                    break;
                case HapticEffectType.Click:
                    durationMs = 18;
                    amplitude = DefaultAmplitude;
                    break;
                case HapticEffectType.HeavyClick:
                case HapticEffectType.Success:
                case HapticEffectType.Error:
                    durationMs = 28;
                    amplitude = DefaultAmplitude;
                    break;
                default:
                    durationMs = 18;
                    amplitude = DefaultAmplitude;
                    break;
            }

            using (AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
            using (AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", durationMs, amplitude))
                vibrator.Call("vibrate", effect);
        }
#endif
    }
}
