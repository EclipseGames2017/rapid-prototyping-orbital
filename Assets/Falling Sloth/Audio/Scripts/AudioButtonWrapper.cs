using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth.Audio
{
    public class AudioButtonWrapper : MonoBehaviour
    {
        public string soundName = "sound1";

        public void PlaySound()
        {
            AudioManager.PlaySound(soundName);
        }

        public void StopSound()
        {
            AudioManager.StopSound(soundName);
        }
    }
}