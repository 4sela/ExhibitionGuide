using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MorseInputHandler : MonoBehaviour
{
    private float pressStartTime;
    private bool isPressing;

    [SerializeField] private float dotThreshold = 1f;

    public event Action<char> OnSymbolDetected;   // Final result
    public event Action<float> OnHolding;         // Raw duration
    public event Action<char> OnPreviewSymbol;    // Live prediction

    private char lastPreviewSymbol;

    private void Update()
    {
        ProcessHolding(Time.time);
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isPressing = true;
            pressStartTime = Time.time;
            lastPreviewSymbol = '\0';
        }
        else if (context.canceled)
        {
            if (!isPressing) return;

            float duration = Time.time - pressStartTime;
            isPressing = false;

            char finalSymbol = duration < dotThreshold ? '.' : '-';

            OnSymbolDetected?.Invoke(finalSymbol);
        }
    }

    private void ProcessHolding(float currentTime)
    {
        if (!isPressing) return;

        float duration = currentTime - pressStartTime;

        OnHolding?.Invoke(duration);

        char previewSymbol = duration < dotThreshold ? '.' : '-';

        if (previewSymbol != lastPreviewSymbol)
        {
            lastPreviewSymbol = previewSymbol;
            OnPreviewSymbol?.Invoke(previewSymbol);
        }
    }

 
}