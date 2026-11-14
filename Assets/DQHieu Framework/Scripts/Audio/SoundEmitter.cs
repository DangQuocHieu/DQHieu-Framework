using System;
using DG.Tweening;
using UnityEngine.Events;

namespace DQHieu.Framework.Audio
{
    using UnityEngine;
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;
        private void OnValidate()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        private void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                _audioSource.playOnAwake = false;
            }
        }
        
        public void PlayAudioClip(AudioClip clip, AudioConfigurationSO settings, bool hasToLoop, Vector3 position = default)
        {
            _audioSource.clip = clip;
            settings.ApplyToAudioSource(_audioSource);
            _audioSource.transform.position = position;
            _audioSource.loop = hasToLoop;
            _audioSource.time = 0f;
            _audioSource.Play();

            if (!hasToLoop)
            {
                DOVirtual.DelayedCall(clip.length, NotifyBeingDone, ignoreTimeScale: false).SetLink(gameObject);
            }
        }

        public void FadeInMusic(AudioClip musicClip, AudioConfigurationSO settings, float duration = 1f,
            float startTime = 0f)
        {
            PlayAudioClip(musicClip, settings, hasToLoop: true);
            _audioSource.volume = 0f;
            if (startTime <= _audioSource.clip.length)
            {
                _audioSource.time = startTime;
            }

            _audioSource.DOFade(settings.Volume, duration).SetLink(gameObject);
        }
        
        public float FadeMusicOut(float duration = 1f)
        {
            _audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;
            return _audioSource.time;
        }

        public void OnFadeOutComplete()
        {
            NotifyBeingDone();
        }

        public void Finish()
        {
            if (_audioSource.loop)
            {
                _audioSource.loop = false;
                float timeRemaining = _audioSource.clip.length - _audioSource.time;
                DOVirtual.DelayedCall(timeRemaining, NotifyBeingDone, ignoreTimeScale: false).SetLink(gameObject);
            }
        }
        
        public void NotifyBeingDone()
        {
            OnSoundFinishedPlaying?.Invoke(this);
        }
        
        public AudioClip GetClip() => _audioSource.clip;
        public void Resume() => _audioSource.Play();
        public void Pause() => _audioSource.Pause();
        public void Stop() => _audioSource.Stop();
        public bool IsPlaying() => _audioSource.isPlaying;
        public bool IsLooping() => _audioSource.loop;
    }
}