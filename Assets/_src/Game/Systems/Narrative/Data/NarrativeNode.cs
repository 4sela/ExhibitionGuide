using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Game.Systems.Narrative.Data
{
    [Serializable]
    public sealed class NarrativeNode
    {
        public string nodeId;             // unique id (author sets)
        [TextArea(3, 10)] public string text;
        public List<NarrativeChoice> choices = new();
        public string defaultTargetNodeId; // if no choices, fallback

        [Header("Audio")]
        public AudioClip voiceClip;   //Text-to-speech
        //public AudioClip blipSound; //If we want typer-writer audio

        [Header("Image")]
        public Texture2D BackgroundImage;

        [Header("Minigame Integration")]
        public GameObject minigamePrefab;
    }
}
