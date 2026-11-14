using UnityEngine.Audio;

namespace DQHieu.Framework.Audio
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "AudioConfigurationSO", menuName = "DQHieuFramework/Audio/AudioConfigurationSO")]
    public class AudioConfigurationSO : ScriptableObject
    {
        public AudioMixerGroup OutputAudioMixerGroup;
        [SerializeField] private PriorityLevel _priorityLevel;
        public int Priority => (int) _priorityLevel;
        public bool Mute = false;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        [Range(-1f, 1f)] public float PanStereo = 0f;
        [Range(0f, 1f)] public float SpatialBlend = 0f;
        public AudioRolloffMode RolloffMode = default;
        [Range(0.01f, 5f)] public float MinDistance = 0f;
        [Range(5f, 100f)] public float MaxDistance = 50f;

        public void ApplyToAudioSource(AudioSource source)
        {
            source.outputAudioMixerGroup = OutputAudioMixerGroup;
            source.mute = Mute;
            source.priority = Priority;
            source.volume = Volume;
            source.pitch = Pitch;
            source.panStereo = PanStereo;
            
            source.spatialBlend = SpatialBlend;
            source.rolloffMode = RolloffMode;
            source.minDistance = MinDistance;
            source.maxDistance = MaxDistance;
        }
    }

    public enum PriorityLevel
    {
        Highest = 0,
        High = 64,
        Standard = 128,
        Low = 194,
        VeryLow = 256
    }

}