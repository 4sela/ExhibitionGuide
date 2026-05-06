namespace Game.Systems.Haptics
{
    public enum HapticEffectType
    {
        Tick, // Sliders, scrubbing, tiny UI feedback
        Click, // Buttons
        HeavyClick, // Important interactions
        DoubleClick,
        Success,
        Error // Wrong/denied
    }
}
