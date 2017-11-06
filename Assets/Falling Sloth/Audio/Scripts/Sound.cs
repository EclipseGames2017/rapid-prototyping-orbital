using UnityEngine;
using UnityEngine.Audio;

namespace FallingSloth.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip audioClip;

        public bool playOnAwake;
        public bool loop;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(.1f, 3f)]
        public float pitch = 1f;
        
        [HideInInspector]
        public AudioSource source;

        public Sound()
        {
            volume = 1f;
            pitch = 1f;
        }
    }
}