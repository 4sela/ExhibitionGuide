using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class VoiceService
{
    private AudioSource source;

    // anti-spam (vigtigt på mobil)
    private float lastPlayTime;
    private float minInterval = 0.03f;

    private float defaultVolume = 1.0f;
    private bool isMuted = false;

    public VoiceService(AudioSource source)
    {
        this.source = source;
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null) return;

        source.Stop();
        source.clip = clip;
        source.Play();
    }

    public void StopVoice()
    {
        if(source.clip == null) return;

        source.Stop();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        source.volume = isMuted ? 0f : defaultVolume;
    }

    public void PauseVoice()
    {
        source.Pause();
    }

    public void UnPause()
    {
        source.UnPause();
    }

    public void ResetVoice()
    {
        source.Stop();
        source.Play();
    }

}