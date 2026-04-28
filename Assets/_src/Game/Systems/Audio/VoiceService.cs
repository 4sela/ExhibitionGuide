using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using Game.Configs;

public sealed class VoiceService
{
    private AudioSource source;
    private float _lastPlayTime;
    private float _minInterval = 0.03f;
    private float _defaultVolume = 1.0f;
    private bool _isMuted = false;

    public VoiceService(AudioSource source)
    {
        this.source = source;
    }

    public void PlayVoiceOnGameStart(AudioClip clip)
    {
        if (clip == null || !GlobalStateEvents.GetDefaultAudioBehaviour.Invoke())
            return;

        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;

        source.clip = clip;
        source.Play();
    }

    public void StopVoice()
    {
        source.Stop();
    }


    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        source.volume = _isMuted ? 0f : _defaultVolume;
    }

    public void PauseVoice()
    {
        if (source.isPlaying)
            source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    public void ResetVoice()
    {
        if (source.clip != null)
        {
            source.Stop();
            source.Play();
        }
    }

    public bool IsPlaying() => source.isPlaying;
    public bool IsPaused() => !source.isPlaying && source.time > 0f;

}
