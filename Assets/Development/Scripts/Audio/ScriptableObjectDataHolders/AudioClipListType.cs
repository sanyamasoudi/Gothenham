using UnityEngine;

namespace Development.Scripts.Audio.ScriptableObjectDataHolders
{
    [CreateAssetMenu(fileName = "AudioClipListType", menuName = "AudioManager/AudioClipListType")]
    public class AudioClipListType : ScriptableObject
    {
        [SerializeField] private string audioClipListName;
        public string GetName() => audioClipListName;
    }
}