using UnityEngine;

namespace Game.Systems.Minigames
{
    public sealed class GameResultController : MonoBehaviour
    {
        [Header("Modals")]
        [SerializeField] private GameObject winModal;
        [SerializeField] private GameObject loseModal;

        public bool IsCompleted { get; set; }

        void Awake()
        {
            if (winModal == null) Debug.LogWarning($"{winModal} is null");
            if (loseModal == null) Debug.LogWarning($"{loseModal} is null");
        }
    }
}
