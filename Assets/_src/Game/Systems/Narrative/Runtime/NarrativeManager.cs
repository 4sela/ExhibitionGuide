using UnityEngine;
using System;
using System.Collections.Generic;
using Game.Systems.Narrative.Data;
using UnityEngine.InputSystem.XR;

namespace Game.Systems.Narrative.Runtime
{
    /// <summary>
    ///
    /// </summary>
    public sealed class NarrativeManager : MonoBehaviour
    {
        public static NarrativeManager Instance { get; private set; }

        [Header("Graph")]
        public NarrativeGraph graph;

        [Tooltip("Start node id")]
        public string startNodeId;

        private Dictionary<string, string> variables = new Dictionary<string, string>();

        public event Action<NarrativeNode> OnNodeEntered;
        public event Action OnNarrativeEnded;

        private NarrativeNode currentNode;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        public void StartNarrative()
        {
            if (graph == null) return;
            GoToNode(startNodeId);
        }

        /// <summary>
        ///
        /// </summary>
        public void GoToNode(string nodeId)
        {
            var node = graph.GetNodeById(nodeId);
            if (node == null)
            {
                Debug.LogWarning($"NarrativeManager: node '{nodeId}' not found.");
                EndNarrative();
                return;
            }

            currentNode = node;
            OnNodeEntered?.Invoke(node);
        }

        /// <summary>
        ///
        /// </summary>
        public void Choose(NarrativeChoice choice)
        {

            if (!string.IsNullOrWhiteSpace(choice.targetNodeId))
            {
                // Safe check: Trim removes accidental spaces you typed in the Inspector
                GoToNode(choice.targetNodeId.Trim());
            }
            else
            {
                Debug.Log("NarrativeManager: Choice clicked, but Target Node ID is empty. Ending story.");
                EndNarrative();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void ContinueDefault()
        {
            if (currentNode == null) return;

            if (!string.IsNullOrWhiteSpace(currentNode.defaultTargetNodeId))
            {
                GoToNode(currentNode.defaultTargetNodeId.Trim());
            }
            else
            {
                Debug.Log("NarrativeManager: Default button clicked, but Default Target Node ID is empty. Ending story.");
                EndNarrative();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool CheckCondition(string key, string expectedValue = null)
        {
            if (!variables.TryGetValue(key, out var val)) return false;
            if (expectedValue == null) return true;
            return val == expectedValue;
        }

        /// <summary>
        ///
        /// </summary>
        private void EndNarrative()
        {
            currentNode = null;
            OnNarrativeEnded?.Invoke();
        }


        public void PlayNode(NarrativeNode node)
        {        
            //Text-to-speech voice clip
            if (node.voiceClip != null)
            {
                AudioManager.Instance.Voice.PlayVoice(node.voiceClip);
            }
        }
    }
}
