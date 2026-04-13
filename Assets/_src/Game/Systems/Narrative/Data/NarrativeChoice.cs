using UnityEngine;
using System;

namespace Game.Systems.Narrative.Data
{
    [Serializable]
    public sealed class NarrativeChoice
    {
        public string id;                 // optional id for analytics
        public string label;              // button text
        public string targetNodeId;       // node to jump to when chosen
    }
}
