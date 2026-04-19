using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;

    private SFXService sfx;
    private VoiceService voice;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfx = new SFXService(sfxSource);
        voice = new VoiceService(voiceSource);
    }

    public SFXService SFX => sfx;
    public VoiceService Voice => voice;
}