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
    private bool _isPlaying = false;
    private bool _isPaused = false;

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
        _isPlaying = true;
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;

        source.Stop();
        source.clip = clip;
        source.Play();
        _isPlaying = true;
        _isPaused = false;
    }

    public void StopVoice()
    {
        if (source.clip == null) return;

        source.Stop();
        _isPlaying = false;
        _isPaused = false;
    }

    public void ToggleMute()
    {
        _isMuted = !_isMuted;
        source.volume = _isMuted ? 0f : _defaultVolume;
    }

    public void PauseVoice()
    {
        if (source.isPlaying)
        {
            source.Pause();
            _isPlaying = false;
            _isPaused = true;
        }
    }

    public void UnPause()
    {
        if (_isPaused)
        {
            source.UnPause();
            _isPlaying = true;
            _isPaused = false;
        }
    }

    public void ResetVoice()
    {
        if (source.clip != null)
        {
            source.Stop();
            source.Play();
            _isPlaying = true;
        }
    }

    public bool IsPaused()
    {
        return _isPaused;
    }

    public bool IsPlaying()
    {
        return _isPlaying;
    }
}
