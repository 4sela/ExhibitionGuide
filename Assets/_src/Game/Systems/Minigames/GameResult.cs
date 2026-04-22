using UnityEngine;
using System;

namespace Game.Systems.Minigames
{
    [Serializable]
    public struct GameResult
    {
        public GameObject winModal;
        public GameObject loseModal;

        public bool IsCompleted { get; set; }

        public void SummonResult()
        {
            if (IsCompleted)
            {
                winModal.SetActive(true);
                return;
            }

            loseModal.SetActive(true);
        }
    }
}
