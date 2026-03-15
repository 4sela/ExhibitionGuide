using UnityEngine;
using System;

namespace Game.Systems.Narrative.Data
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public sealed class NarrativeChoice
    {
        public string id;                 // optional id for analytics
        public string label;              // button text
        public string targetNodeId;       // node to jump to when chosen
        public string conditionKey;       // optional variable key to require
        public string setVariableKey;     // optional variable to set on choose
        public string setVariableValue;   // value to set
    }
}
