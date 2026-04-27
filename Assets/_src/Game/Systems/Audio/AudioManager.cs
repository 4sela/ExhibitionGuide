using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource sfxSource;
    public AudioSource voiceSource;

    private SFXService _sfx;
    private VoiceService _voice;

    public SFXService SFX => _sfx;
    public VoiceService Voice => _voice;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _sfx = new SFXService(sfxSource);
        _voice = new VoiceService(voiceSource);
    }
}
