using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.Audio
{
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        public List<Sound> sounds;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
                return;
            
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.audioClip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.playOnAwake = sound.playOnAwake;
                sound.source.loop = sound.loop;

                if (sound.playOnAwake)
                    sound.source.Play();
            }
        }

        public static void PlaySound(string name)
        {
            foreach (Sound sound in Instance.sounds)
            {
                if (sound.name.ToLower() == name.ToLower())
                {
                    sound.source.Play();
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException("name", "No sound with the given name found.");
        }

        public static void StopSound(string name)
        {
            foreach (Sound sound in Instance.sounds)
            {
                if (sound.name.ToLower() == name.ToLower())
                {
                    sound.source.Stop();
                    return;
                }
            }

            throw new System.ArgumentOutOfRangeException("name", "No sound with the given name found.");
        }
    }
}