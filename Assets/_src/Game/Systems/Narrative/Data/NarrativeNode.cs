using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Systems.Narrative.Data
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public sealed class NarrativeNode
    {
        public string nodeId;             // unique id (author sets)
        [TextArea(3, 10)] public string text;
        public List<NarrativeChoice> choices = new();
        public string defaultTargetNodeId; // if no choices, fallback
        public float autoAdvanceDelay = 0f; // 0 = wait for user
        public string onEnterEvent;       // optional event name to fire
    }
}
