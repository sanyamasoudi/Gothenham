using UnityEngine;

namespace Development.Scripts.Audio.ScriptableObjectDataHolders
{
    [CreateAssetMenu(fileName = "AudioChannel", menuName = "AudioManager/AudioChannel")]
    public class AudioChannel : ScriptableObject
    {
        [SerializeField] private string audioChannelName;
        [SerializeField] [Range(0, 1)] private float volume;
        
        public string GetName() => audioChannelName;
        public float GetVolume() => volume;
    }
}