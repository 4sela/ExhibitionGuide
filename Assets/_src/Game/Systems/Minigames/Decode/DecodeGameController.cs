using UnityEngine;
using Game.Systems.Narrative.Events;

namespace Systems.Minigames.Decode
{
    public sealed class DecodeGameController : MonoBehaviour
    {
        private void Awake()
        {
            AudioManager.Instance.Voice.StopVoice();
        }

        public void OnMinigameCompleted()
        {
            NarrativeEvents.MiniGameComplete?.Invoke();
        }
    }
}
