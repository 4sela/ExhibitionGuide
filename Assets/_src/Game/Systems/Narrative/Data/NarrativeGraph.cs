using UnityEngine;
using System.Collections.Generic;

namespace Game.Systems.Narrative.Data
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Narrative Graph", fileName = "Nar_XXX (X.X.X)")]
    public sealed class NarrativeGraph : ScriptableObject
    {
        public List<NarrativeNode> nodes = new List<NarrativeNode>();

        /// <summary>
        ///
        /// </summary>
        public NarrativeNode GetNodeById(string id)
        {
            return nodes.Find(n => n.nodeId != null && n.nodeId.Trim() == id.Trim());
        }
    }
}
