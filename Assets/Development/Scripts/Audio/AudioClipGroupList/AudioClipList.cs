using System;
using System.Collections.Generic;
using Development.Scripts.Audio.AudioClip;
using Development.Scripts.Audio.ScriptableObjectDataHolders;
using UnityEngine;

namespace Development.Scripts.Audio.AudioClipGroupList
{
    [Serializable]
    public class AudioClipList
    {
        [HideInInspector] public string name;
        public AudioClipListType type;
        public bool useChannel;
        public AudioChannel audioChannel;
        public List<AudioClipData> audioClips;
    }
}