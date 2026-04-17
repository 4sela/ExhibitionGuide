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
                    break;

                case 1:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(true);
                    linzSkibet.SetActive(false);
                    break;

                case 2:
                    odinsTårnet.SetActive(false);
                    augustOprøret.SetActive(false);
                    linzSkibet.SetActive(true);
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
