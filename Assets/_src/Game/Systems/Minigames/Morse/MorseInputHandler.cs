using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MorseInputHandler : MonoBehaviour
{
    private float pressStartTime;
    private bool isPressing;

    [SerializeField] private float dotThreshold = 0.35f;

    public event Action<char> OnSymbolDetected;   // Final result
    public event Action<float> OnHolding;         // Raw duration
    public event Action<char> OnPreviewSymbol;    // Live prediction

    private char lastPreviewSymbol;

    private void Update()
    {
        ProcessHolding(Time.time);
    }

    //INPUT SYSTEM (keyboard/controller)
    public void OnPress(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            StartPress();
        }
        else if (context.canceled)
        {
            EndPress();
        }
    }

    //UI BUTTON (mobile)
    public void OnButtonDown()
    {
        StartPress();
    }

    public void OnButtonUp()
    {
        EndPress();
    }

    //Shared logic (used by both systems)
    private void StartPress()
    {
        isPressing = true;
        pressStartTime = Time.time;
        lastPreviewSymbol = '\0';
        MorseSequenceBuilder.Instance.SetPressing(true);
    }

    private void EndPress()
    {
        if (!isPressing) return;

        float duration = Time.time - pressStartTime;
        isPressing = false;

        char finalSymbol = duration < dotThreshold ? '.' : '-';
        MorseSequenceBuilder.Instance.SetPressing(false);
        MorseSequenceBuilder.Instance.AddSymbol(finalSymbol, Time.time);
        //char previewSymbol = MorseSequenceBuilder.Instance.GetCurrentLetter();
        //OnPreviewSymbol?.Invoke(previewSymbol);


        OnSymbolDetected?.Invoke(finalSymbol);
    }

    private void ProcessHolding(float currentTime)
    {
        if (!isPressing) return;

        float duration = currentTime - pressStartTime;

        OnHolding?.Invoke(duration);

        char previewSymbol = duration < dotThreshold * 0.95f ? '.' : '-';

        if (previewSymbol != lastPreviewSymbol)
        {
            lastPreviewSymbol = previewSymbol;
            OnPreviewSymbol?.Invoke(previewSymbol);
        }
    }
}