namespace DQHieu.Framework.Audio
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AudioCueSO", menuName = "DQHieuFramework/Audio/AudioCueSO")]
    public class AudioCueSO : ScriptableObject
    {
        public bool Looping = false;
        [SerializeField] private AudioClipGroup[] _audioClipGroups = default;

        public AudioClip[] GetClips()
        {
            int numberOfClips = _audioClipGroups.Length;
            AudioClip[] results = new AudioClip[numberOfClips];
            for (int i = 0; i < numberOfClips; i++)
            {
                results[i] = _audioClipGroups[i].GetNextClip();
            }

            return results;
        }
    }

    [System.Serializable]
    public class AudioClipGroup
    {
        public SequenceMode SequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] AudioClips;
        private int _nextClipToPlay = -1;
        private int _lastClipToPlay = -1;

        public AudioClip GetNextClip()
        {
            if (AudioClips == null || AudioClips.Length == 0)
            {
                Debug.Log("No audio clips found !");
                return null;
            }
            if (AudioClips.Length == 1) return AudioClips[0];
            if (_nextClipToPlay == -1) //Cue này được phát lần đầu khi bật game
            {
                if (SequenceMode == SequenceMode.Sequential)
                {
                    _nextClipToPlay = 0;
                }
                else
                {
                    _nextClipToPlay = UnityEngine.Random.Range(0, AudioClips.Length);
                }
            }
            else
            {
                switch (SequenceMode)
                {
                    case SequenceMode.Random:
                        _nextClipToPlay = UnityEngine.Random.Range(0, AudioClips.Length);
                        break;
                    case SequenceMode.Sequential:
                        _nextClipToPlay = (_nextClipToPlay + 1) %  AudioClips.Length;
                        break;
                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            _nextClipToPlay = UnityEngine.Random.Range(0, AudioClips.Length);
                        } while (_nextClipToPlay == _lastClipToPlay);
                        break;
                }
            }

            _lastClipToPlay = _nextClipToPlay;
            return AudioClips[_nextClipToPlay];
        }
    }

    public enum SequenceMode
    {
        Random, //ngẫu nhiên
        RandomNoImmediateRepeat, //ngẫu nhiên không trùng âm thanh trước
        Sequential //lặp vòng tròn
    }
}