using UnityEngine;
using Game.Systems.Narrative.Events;

namespace Systems.Minigames.Decode
{
    public sealed class DecodeGameController : MonoBehaviour
    {
        public void OnMinigameCompleted()
        {
            NarrativeEvents.MiniGameComplete?.Invoke();
        }
    }
}
