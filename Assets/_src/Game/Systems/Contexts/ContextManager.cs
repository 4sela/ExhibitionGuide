using UnityEngine;

namespace Game.Systems.Contexts
{
    public sealed class ContextManager : MonoBehaviour
    {
        public static ContextManager Instance { get; private set; }

        [Header("Odinstårnet")]
        [SerializeField] private GameObject odinsTårnet;
        [SerializeField] private AudioClip audioOdinsTårnet;

        [Header("Augustoprøret")]
        [SerializeField] private GameObject augustOprøret;
        [SerializeField] private AudioClip audioAugustOprøret;

        [Header("Linz Skibet")]
        [SerializeField] private GameObject linzSkibet;
        [SerializeField] private AudioClip audioLinzSkibet;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void LoadContextPanel(int contextPanel)
        {
            AudioManager.Instance.Voice.StopVoice();

            switch (contextPanel)
            {
                case 0:
                    odinsTårnet.SetActive(true);
                    augustOprøret.SetActive(false);
                    linzSkibet.SetActive(false);

                    if (audioOdinsTårnet != null)
                        Invoke(nameof(PlayOdinsTårnet), 1f);

                    break;

                case 1:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(true);
                    linzSkibet.SetActive(false);

                    if (audioAugustOprøret != null)
                        Invoke(nameof(PlayAugustOprøert), 1f);

                    break;

                case 2:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(false);
                    linzSkibet.SetActive(true);

                    if (audioLinzSkibet != null)
                        Invoke(nameof(PlayLinzSkibet), 1f);

                    break;
            }
        }

        public void CloseAllPanels()
        {
            odinsTårnet.SetActive(false);
            augustOprøret.SetActive(false);
            linzSkibet.SetActive(false);
        }

        private void PlayOdinsTårnet()
        {
            AudioManager.Instance.voiceSource.clip = audioOdinsTårnet;
            AudioManager.Instance.Voice.PlayVoiceOnGameStart(audioOdinsTårnet);
        }

        private void PlayAugustOprøert()
        {
            AudioManager.Instance.voiceSource.clip = audioAugustOprøret;
            AudioManager.Instance.Voice.PlayVoiceOnGameStart(audioAugustOprøret);
        }

        private void PlayLinzSkibet()
        {
            AudioManager.Instance.voiceSource.clip = audioLinzSkibet;
            AudioManager.Instance.Voice.PlayVoiceOnGameStart(audioLinzSkibet);
        }
    }
}
