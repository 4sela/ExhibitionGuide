using UnityEngine;
using System;

namespace Game.Systems.Player
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PlayerNameSystem : MonoBehaviour
    {
        private string _playerName;

        void OnEnable()
        {
            PlayerEvents.SetName += SetName;
            PlayerEvents.GetName += GetName;
        }

        void OnDisable()
        {
            PlayerEvents.SetName -= SetName;
            PlayerEvents.GetName -= GetName;
        }

        private void SetName(string newName) => _playerName = newName;
        private string GetName() => _playerName;
    }
}
