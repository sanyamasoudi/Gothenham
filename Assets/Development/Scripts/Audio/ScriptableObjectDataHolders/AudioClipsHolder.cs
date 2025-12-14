using System.Collections.Generic;
using Development.Scripts.Audio.AudioClipGroupList;
using UnityEngine;

namespace Development.Scripts.Audio.ScriptableObjectDataHolders
{
    [CreateAssetMenu(fileName = "AudioClipsHolder", menuName = "AudioManager/AudioClipsHolder")]
    public class AudioClipsHolder : ScriptableObject
    {
        [SerializeField] private List<AudioClipList> audioClips;
        private string _audioClipsName;

        private void OnValidate()
        {
            foreach (var audioClip in audioClips)
            {
                _audioClipsName = audioClip.type?.GetName();
                audioClip.name = _audioClipsName == string.Empty ? "Default" : _audioClipsName;
            }
        }
        
        public List<AudioClipList> GetAudioClips() => audioClips;
        
    }
}