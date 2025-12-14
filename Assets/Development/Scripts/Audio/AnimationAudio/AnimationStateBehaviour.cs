using Development.Scripts.Audio.Core;
using UnityEngine;

namespace Development.Scripts.Audio.AnimationAudio
{
    public class AnimationStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string audioClipName;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.Instance.PlaySequentialAudioTrack(audioClipName);
        }
    
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            AudioManager.Instance.StopAudioTrack(audioClipName);
        }
    
    }
}
