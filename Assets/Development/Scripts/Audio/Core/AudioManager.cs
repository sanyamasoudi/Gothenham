using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Development.Scripts.Audio.AudioClip;
using Development.Scripts.Audio.AudioClipGroupList;
using Development.Scripts.Audio.ScriptableObjectDataHolders;
using UnityEngine;

namespace Development.Scripts.Audio.Core
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClipsHolder audioClipsHolder;
        [SerializeField] private int audioPoolInitSize = 2;

        private static AudioManager _instance;
        public static AudioManager Instance => _instance;

        private List<AudioSource> _audioPool;
        private List<AudioClipList> _audioClipLists;
        private Dictionary<string, List<AudioSource>> _audioSourcesByTrack;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                InitializeAudioPool();
            }
        }

        private void Start()
        {
            _audioClipLists = audioClipsHolder.GetAudioClips();
            _audioSourcesByTrack = new Dictionary<string, List<AudioSource>>();
            PlayStartAudios();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            foreach (var source in _audioPool)
            {
                if (source != null) source.Stop();
            }
        }


        #region Public Playback API

        public void PlaySequentialAudioTrack(string audioTrackType)
        {
            var audioClipList = GetListByTrack(audioTrackType);
            if (audioClipList == null) return;

            StartCoroutine(PlaySequential(audioClipList));
        }

        public void PlayParallelAudioTrack(string audioTrackType)
        {
            var audioClipList = GetListByTrack(audioTrackType);
            if (audioClipList == null) return;

            StartCoroutine(PlayParallel(audioClipList));
        }

        public void PlayRandomFromAudioTrack(string audioTrackType)
        {
            var audioClipList = GetListByTrack(audioTrackType);
            if (audioClipList == null) return;

            int index = Random.Range(0, audioClipList.audioClips.Count);
            var randomClip = audioClipList.audioClips[index];

            StartCoroutine(PlayAudioClip(randomClip, audioClipList));
        }

        #endregion

        #region Public Fade API

        public void PlayFadeInAudioTrack(string audioTrackType, float duration)
        {
            var audioClipList = GetListByTrack(audioTrackType);
            foreach (var audioClipData in audioClipList.audioClips)
            {
                var audioSource = PrepareAudioSource(audioClipData, audioClipList);
                StartCoroutine(FadeInCoroutine(audioSource, duration));
            }
        }

        public void PlayFadeOutAudioTrack(string audioTrackType, float duration)
        {
            var audioClipList = GetListByTrack(audioTrackType);
            foreach (var audioClipData in audioClipList.audioClips)
            {
                var audioSource = PrepareAudioSource(audioClipData, audioClipList);
                StartCoroutine(FadeOutCoroutine(audioSource, duration));
            }
        }

        public void PlayFadeOutExistedAudioTrack(string audioTrackType, float duration, float targetVolume = 0f)
        {
            if (!_audioSourcesByTrack.ContainsKey(audioTrackType))
            {
                return;
            }

            foreach (var audioSource in _audioSourcesByTrack[audioTrackType])
            {
                if (audioSource.isPlaying)
                {
                    StartCoroutine(FadeOutCoroutine(audioSource,duration , targetVolume));
                }
            }
        }

        private IEnumerator FadeInCoroutine(AudioSource audioSource, float duration, float targetVolume = 0f)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, elapsedTime / duration);
                yield return null;
            }

            audioSource.volume = targetVolume;
        }

        private IEnumerator FadeOutCoroutine(AudioSource audioSource, float duration, float startVolume = 0f)
        {
            var elapsedTime = 0f;
            var targetVolume = audioSource.volume;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / duration);
                yield return null;
            }

            audioSource.volume = targetVolume;
        }

        #endregion


        #region Public Track Control (Stop, Mute, Pause)

        public void StopAudioTrack(string audioTrackType) =>
            ApplyToTrackSources(audioTrackType, s => s.Stop());

        public void MuteAudioTrack(string audioTrackType) =>
            ApplyToTrackSources(audioTrackType, s => s.mute = true);

        public void UnMuteAudioTrack(string audioTrackType) =>
            ApplyToTrackSources(audioTrackType, s => s.mute = false);

        public void PauseAudioTrack(string audioTrackType) =>
            ApplyToTrackSources(audioTrackType, s => s.Pause());

        public void UnPauseAudioTrack(string audioTrackType) =>
            ApplyToTrackSources(audioTrackType, s => s.UnPause());

        public bool IsAudioTrackPlaying(string audioTrackType)
        {
            if (!_audioSourcesByTrack.ContainsKey(audioTrackType))
                return false;

            return _audioSourcesByTrack[audioTrackType].Any(s => s.isPlaying);
        }

        public void SetVolumeAudioTrack(string audioTrackType, float volume)
        {
            if (!_audioSourcesByTrack.ContainsKey(audioTrackType))
            {
                return;
            }

            foreach (var audioSource in _audioSourcesByTrack[audioTrackType])
            {
                if (audioSource.isPlaying)
                {
                    audioSource.volume = volume;
                }
            }
        }
        

        #endregion


        #region Public Global Control

        public void StopAllAudios() => _audioPool.ForEach(s => s.Stop());
        public void MuteAllAudios() => _audioPool.ForEach(s => s.mute = true);
        public void UnMuteAllAudios() => _audioPool.ForEach(s => s.mute = false);
        public void PauseAllAudios() => _audioPool.ForEach(s => s.Pause());
        public void UnPauseAllAudios() => _audioPool.ForEach(s => s.UnPause());

        #endregion


        private void InitializeAudioPool()
        {
            _audioPool = new List<AudioSource>();

            for (int i = 0; i < audioPoolInitSize; i++)
                CreateNewAudioSource();
        }

        private void PlayStartAudios()
        {
            foreach (var list in _audioClipLists)
            {
                foreach (var clip in list.audioClips)
                {
                    if (!clip.playOnAwake) continue;

                    var src = PrepareAudioSource(clip, list);
                    if (clip.loop) src.Play();
                    else src.PlayOneShot(src.clip);
                }
            }
        }

        private AudioSource CreateNewAudioSource()
        {
            var src = gameObject.AddComponent<AudioSource>();
            _audioPool.Add(src);
            return src;
        }

        private AudioSource GetFreeAudioSource()
        {
            foreach (var audioSource in _audioPool)
            {
                if (!audioSource.isPlaying)
                {
                    return audioSource;
                }
            }

            return CreateNewAudioSource();
        }

        private IEnumerator PlaySequential(AudioClipList audioClipList)
        {
            foreach (var clip in audioClipList.audioClips)
            {
                yield return StartCoroutine(PlayAudioClip(clip, audioClipList));
            }
        }

        private IEnumerator PlayParallel(AudioClipList audioClipList)
        {
            foreach (var clip in audioClipList.audioClips)
            {
                StartCoroutine(PlayAudioClip(clip, audioClipList));
            }

            yield break;
        }

        private IEnumerator PlayAudioClip(AudioClipData clipData, AudioClipList list)
        {
            var audioSource = PrepareAudioSource(clipData, list);

            if (clipData.loop) audioSource.Play();
            else if (clipData.playAfterSeconds > 0) audioSource.PlayDelayed(clipData.playAfterSeconds);
            else if (clipData.isOneShot) audioSource.PlayOneShot(clipData.clip);
            else audioSource.Play();

            yield return new WaitWhile(() => audioSource.isPlaying);
        }

        private AudioSource PrepareAudioSource(AudioClipData clip, AudioClipList list)
        {
            var src = GetFreeAudioSource();
            if (src.clip)
            {
                UnRegisterTrackSound(src);
            }

            RegisterTrackSource(list.type.GetName(), src);

            src.playOnAwake = clip.playOnAwake;
            src.loop = clip.loop;
            src.mute = clip.mute;
            src.clip = clip.clip;

            src.pitch = clip.isRandomPitch ? Random.Range(0.1f, 1f) : clip.pitch;
            src.panStereo = clip.isRandomPan ? Random.Range(-1f, 1f) : clip.stereoPan;

            SetVolume(clip, list, src);

            return src;
        }

        private static void SetVolume(AudioClipData clip, AudioClipList audioClipList, AudioSource audioSource)
        {
            if (audioClipList.useChannel)
            {
                audioSource.volume = audioClipList.audioChannel.GetVolume();
            }
            else
            {
                audioSource.volume = clip.isRandomVolume ? Random.Range(0.5f, 1f) : clip.volume;
            }
        }

        private void UnRegisterTrackSound(AudioSource source)
        {
            List<string> unRegisterTrackNames = new List<string>();
            foreach (var track in _audioSourcesByTrack)
            {
                foreach (var audioSource in track.Value)
                {
                    if (audioSource == source)
                    {
                        unRegisterTrackNames.Add(track.Key);
                    }
                }
            }

            foreach (var trackName in unRegisterTrackNames)
            {
                _audioSourcesByTrack.Remove(trackName);
            }
        }

        private void RegisterTrackSource(string trackName, AudioSource source)
        {
            if (!_audioSourcesByTrack.TryGetValue(trackName, out var audioSources))
            {
                audioSources = new List<AudioSource>();
                _audioSourcesByTrack.Add(trackName, audioSources);
            }

            if (!audioSources.Contains(source))
            {
                audioSources.Add(source);
            }
        }

        private void ApplyToTrackSources(string trackName, System.Action<AudioSource> action)
        {
            if (_audioSourcesByTrack.TryGetValue(trackName, out var sources))
            {
                foreach (var source in sources)
                {
                    action(source);
                }
            }
        }

        private AudioClipList GetListByTrack(string trackName)
        {
            var list = _audioClipLists.FirstOrDefault(l => l.type.GetName() == trackName);
            if (list == null)
            {
                Debug.LogError($"No audio clips found for {trackName}");
            }

            return list;
        }
    }
}