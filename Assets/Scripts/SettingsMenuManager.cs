using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class SettingsMenuManager : MonoBehaviour
    {
        public Slider volumeSlider;

        void Start()
        {
            //ChangeGlobalVolume(SaveDataManager.data.volume);
            volumeSlider.value = SaveDataManager.data.volume;
        }

        public void ChangeGlobalVolume(float volume)
        {
            Debug.Log("Changing global volume!");

            AudioManager.Instance.ChangeGlobalVolume(volume);

            SaveDataManager.data.volume = volume;
            SaveDataManager.SaveData();
        }
    }
}