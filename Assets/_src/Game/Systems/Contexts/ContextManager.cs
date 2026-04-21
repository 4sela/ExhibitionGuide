using UnityEngine;

namespace Game.Systems.Contexts
{
    public sealed class ContextManager : MonoBehaviour
    {
        public static ContextManager Instance { get; private set; }

        [Header("Context Panels")]
        [SerializeField] private GameObject odinsTårnet;
        [SerializeField] private GameObject augustOprøret;
        [SerializeField] private GameObject linzSkibet;

        [SerializeField] private AudioClip audio_OdinsTårnet;
        [SerializeField] private AudioClip audio_AugustOprøret;
        [SerializeField] private AudioClip audio_LinzSkibet;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void LoadContextPanel(int contextPanel)
        {
            switch (contextPanel)
            {
                case 0:
                    odinsTårnet.SetActive(true);
                    augustOprøret.SetActive(false);
                    linzSkibet.SetActive(false);

                    if(audio_OdinsTårnet != null)
                    {
                        AudioManager.Instance.Voice.PlayVoice(audio_OdinsTårnet);
                    }                    
                    break;

                case 1:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(true);
                    linzSkibet.SetActive(false);

                    if(audio_AugustOprøret != null)
                    {
                        AudioManager.Instance.Voice.PlayVoice(audio_AugustOprøret);
                    }                    
                    break;

                case 2:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(false);
                    linzSkibet.SetActive(true);

                    if(audio_LinzSkibet != null)
                    {
                        AudioManager.Instance.Voice.PlayVoice(audio_LinzSkibet);
                    }                    
                    break;
            }
        }

        public void CloseAllPanels()
        {
            odinsTårnet.SetActive(false);
            augustOprøret.SetActive(false);
            linzSkibet.SetActive(false);
        }
    }
}
