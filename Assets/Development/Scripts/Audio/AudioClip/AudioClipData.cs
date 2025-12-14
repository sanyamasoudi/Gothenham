using System;
using UnityEngine;

namespace Development.Scripts.Audio.AudioClip
{
    [Serializable]
    public class AudioClipData
    {
        [Header("Audio Clip")]
        public UnityEngine.AudioClip clip;
        public float playAfterSeconds = 0f;
        
        [Header("Audio Parameters")] 
        
        [Space(5)]
        [Header("Volume")] 
        [Range(0, 1)] public float volume = 0.5f;
        public bool isRandomVolume = false;
        
        [Space(5)]
        [Header("Pitch")] 
        [Range(-3, 3)] public float pitch = 1f;
        public bool isRandomPitch = false;
        
        [Space(5)]
        [Header("StereoPan")] 
        [Range(-1, 1)] public float stereoPan = 0f;
        public bool isRandomPan = false;
        
        [Header("Audio Status")] 
        public bool playOnAwake = false;
        public bool loop = false;
        public bool mute = false;
        public bool isOneShot = false;
    }
}