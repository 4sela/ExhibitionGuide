using UnityEngine;
using System;
using Game.Systems.Minigames;
using Game.Systems.Narrative.Events;
using Game.Systems.Narrative.Runtime;

namespace Systems.Minigames.Decode
{
    public sealed class DecodeGameController : MonoBehaviour, IMinigame
    {
        [SerializeField] private GameResult gameResult;

        // NOTE: Ambigious name
        public void ButtonPress(bool arg)
        {
            gameResult.IsCompleted = arg;
            gameResult.SummonResult();
        }

        public void ProceedToNarrativeScreen()
        {
            NarrativeManager.Instance.ContinueDefault();
        }
    }
}
