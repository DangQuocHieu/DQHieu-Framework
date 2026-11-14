using UnityEngine;
using UnityEngine.Audio;
using DQHieu.Framework.DesignPattern;

namespace DQHieu.Framework.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Prefab Chiếc Loa (Để Pool mượn)")]
        [SerializeField] private SoundEmitter _soundEmitterPrefab;

        [Header("Audio Mixer & Âm Lượng")]
        [SerializeField] private AudioMixer _audioMixer;
        [Range(0f, 1f)] [SerializeField] private float _masterVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float _musicVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float _sfxVolume = 1f;

        // Giữ riêng 1 chiếc loa chuyên phát nhạc nền
        private SoundEmitter _musicSoundEmitter;

        // 1. PHÁT TIẾNG ĐỘNG (SFX: Chém, Nhảy, Bước chân, UI...)
        public void PlaySFX(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clips = audioCue.GetClips();

            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null) continue;

                // Mượn 1 chiếc loa từ ObjectPoolManager của bạn
                SoundEmitter emitter = ObjectPoolManager.Instance.Get(_soundEmitterPrefab);
                emitter.PlayAudioClip(clips[i], settings, audioCue.Looping, position);

                // Nếu tiếng không lặp -> Khi phát xong tự động trả loa về Pool
                if (!audioCue.Looping)
                {
                    emitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
        }

        // Khi loa báo phát xong -> Thu hồi loa về Pool
        private void OnSoundEmitterFinishedPlaying(SoundEmitter emitter)
        {
            emitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            emitter.Stop();
            ObjectPoolManager.Instance.Release(emitter);
        }

        // 2. PHÁT NHẠC NỀN (MUSIC)
        public void PlayMusic(AudioCueSO musicCue, AudioConfigurationSO settings, float fadeDuration = 1.5f)
        {
            AudioClip newSong = musicCue.GetClips()[0];
            float startTime = 0f;

            // Nếu đang có nhạc nền kêu -> Cho bài cũ nhỏ dần (Fade Out)
            if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
            {
                if (_musicSoundEmitter.GetClip() == newSong) return; // Trùng bài đang phát thì thôi
                startTime = _musicSoundEmitter.FadeMusicOut(fadeDuration);
            }

            // Mượn 1 loa mới cho bài nhạc mới to dần lên (Fade In)
            _musicSoundEmitter = ObjectPoolManager.Instance.Get(_soundEmitterPrefab);
            _musicSoundEmitter.FadeInMusic(newSong, settings, fadeDuration, startTime);
            _musicSoundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
        }

        public void StopMusic()
        {
            if (_musicSoundEmitter != null && _musicSoundEmitter.IsPlaying())
            {
                OnSoundEmitterFinishedPlaying(_musicSoundEmitter);
                _musicSoundEmitter = null;
            }
        }

        // 3. CHỈNH ÂM LƯỢNG (0 -> 1)
        public void SetMasterVolume(float volume01)
        {
            _masterVolume = Mathf.Clamp01(volume01);
            if (_audioMixer != null) _audioMixer.SetFloat("MasterVolume", (_masterVolume - 1f) * 80f);
        }

        public void SetMusicVolume(float volume01)
        {
            _musicVolume = Mathf.Clamp01(volume01);
            if (_audioMixer != null) _audioMixer.SetFloat("MusicVolume", (_musicVolume - 1f) * 80f);
        }

        public void SetSFXVolume(float volume01)
        {
            _sfxVolume = Mathf.Clamp01(volume01);
            if (_audioMixer != null) _audioMixer.SetFloat("SFXVolume", (_sfxVolume - 1f) * 80f);
        }
    }
}