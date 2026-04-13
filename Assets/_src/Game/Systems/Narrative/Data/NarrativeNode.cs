using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Systems.Narrative.Data
{
    [Serializable]
    public sealed class NarrativeNode
    {
        public string nodeId;             // unique id (author sets)
        [TextArea(3, 10)] public string text;
        public List<NarrativeChoice> choices = new();
        public string defaultTargetNodeId; // if no choices, fallback

        [Header("Minigame Integration")]
        public GameObject minigamePrefab;
    }
}
