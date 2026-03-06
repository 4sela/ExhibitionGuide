using UnityEngine;
using System;

namespace Game.Systems.Player
{
    public sealed class PlayerNameSystem : MonoBehaviour
    {
        public static PlayerNameSystem Instance;

        public string PlayerName { get; private set; }
        public event Action<string> OnNameChanged;

        void Awake()
        {
            Instance = this;
        }

        public void SetName(string newName)
        {
            PlayerName = newName;
            OnNameChanged?.Invoke(newName);
        }
    }
}
