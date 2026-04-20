using UnityEngine;


public class SFXService 
{
    private AudioSource source;

    public SFXService(AudioSource source)
    {
        this.source = source;
    }

    // UI + generelle lyde
    public void Play(AudioClip clip, float volume, float pitchVariation = 0.05f) //Volume 0 to 1
    {
        if (clip == null) return;

        source.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        source.PlayOneShot(clip, volume);
    }
}
