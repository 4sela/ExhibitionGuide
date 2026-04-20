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

    public VoiceService(AudioSource source)
    {
        this.source = source;
    }

    //Typewriter-lyd?
    //public IEnumerator TypeText(string text, TMP_Text uiText, AudioClip blip, float delay = 0.03f)
    //{
    //    uiText.text = "";

    //    foreach (char c in text)
    //    {
    //        uiText.text += c;

    //        if (!char.IsWhiteSpace(c) && blip != null)
    //        {
    //            if (Time.time - lastPlayTime > minInterval)
    //            {
    //                source.PlayOneShot(blip, 0.5f);
    //                lastPlayTime = Time.time;
    //            }
    //        }

    //        yield return new WaitForSeconds(delay);
    //    }
    //}

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

}