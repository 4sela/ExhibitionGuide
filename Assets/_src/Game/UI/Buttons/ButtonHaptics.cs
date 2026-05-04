using UnityEngine;
using UnityEngine.UI;
using Game.Systems.Haptics;

namespace Game.UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public sealed class ButtonHaptics : MonoBehaviour
    {
        [Header("Haptics Settings")]
        [SerializeField] private HapticEffectType effectType = HapticEffectType.Tick;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlayHaptic);
        }

        private void OnDestroy()
        {
            if (_button != null)
                _button.onClick.RemoveListener(PlayHaptic);
        }

        private void PlayHaptic()
        {
            HapticsService.Play(effectType);
        }
    }
}
