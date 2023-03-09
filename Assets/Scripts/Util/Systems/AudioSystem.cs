using PotionsPlease.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PotionsPlease.Util.Systems
{
    public class AudioSystem : SystemBase<AudioSystem>
    {
        [System.Serializable]
        private class Sound
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: SerializeField] public AudioClip Clip { get; private set; }
            [field: SerializeField, Range(0, 1)] public float Volume { get; private set; } = 0.5f;
            [field: SerializeField, Range(0, 2)] public float Pitch { get; private set; } = 1;
            [field: SerializeField] public bool Loop { get; private set; } = false;
            public AudioSource AudioSource { get; set; }
        }

        [SerializeField] private AudioSource _soundtrackAudioSource;
        [SerializeField] private AudioLowPassFilter _audioLowPassFilter;
        [SerializeField] private float _menuStateFrequency;
        [SerializeField] private float _inGameStateFrequency;
        private float _soundtrackTargetFrequency;

        [SerializeField] private List<Sound> _sounds;
        private Dictionary<string, Sound> _soundSourcesDict { get; set; }

        private static bool _playSounds = true;
        
        private void Start()
        {
            _soundSourcesDict = _sounds.ToDictionary(sound => sound.Name, sound => sound);

            foreach (var sound in _soundSourcesDict.Values)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.Clip;
                audioSource.volume = sound.Volume;
                audioSource.pitch = sound.Pitch;
                audioSource.loop = sound.Loop;
                sound.AudioSource = audioSource;
            }
        }

        private void Update()
        {
            var frequncyCurrent = _audioLowPassFilter.cutoffFrequency;
            if (frequncyCurrent != _soundtrackTargetFrequency)
            {
                _audioLowPassFilter.cutoffFrequency = Mathf.Lerp(frequncyCurrent, _soundtrackTargetFrequency, 10f * Time.deltaTime);
            }
        }

        public static void PlaySound(string name, float pitchRandomize = 0)
        {
            if (!_playSounds)
                return;

            if (Instance._soundSourcesDict.TryGetValue(name, out var sound))
            {
                sound.AudioSource.pitch = sound.Pitch + (pitchRandomize == 0 ? 0 : Random.Range(-pitchRandomize, pitchRandomize));
                sound.AudioSource.volume = sound.Volume;
                sound.AudioSource.Play();
            }
            else
                Debug.LogError($"Sound {name} not found.");
        }

        public static void StopSound(string name)
        {
            if (Instance._soundSourcesDict.TryGetValue(name, out var sound))
                sound.AudioSource.Stop();
            else
                Debug.LogError($"Sound {name} not found.");
        }

        public void ToggleMusic(bool value) => _soundtrackAudioSource.mute = !value;

        public void ToggleSounds(bool value) => _playSounds = value;

        public void SetSoundtrackMenuState(bool value, bool setInstantly = false)
        {
            _soundtrackTargetFrequency = value ? _menuStateFrequency : _inGameStateFrequency;

            if (setInstantly)
                _audioLowPassFilter.cutoffFrequency = _soundtrackTargetFrequency;
        }
    }
}