using Development.Scripts.Audio.Core;
using Development.Scripts.Audio.ScriptableObjectDataHolders;

namespace Development.Scripts.Audio.AnimationAudio
{
    public static class AnimationAudioStateManager
    {
        public static void OnStatePlay(string audioClipListType)
        {
            if(AudioManager.Instance.IsAudioTrackPlaying(audioClipListType)) return;
            AudioManager.Instance.PlaySequentialAudioTrack(audioClipListType);
        }

        public static void OnStateStop(string audioClipListType)
        {
            if(AudioManager.Instance.IsAudioTrackPlaying(audioClipListType))
            {
                AudioManager.Instance.StopAudioTrack(audioClipListType);
            }
        }
    }
}