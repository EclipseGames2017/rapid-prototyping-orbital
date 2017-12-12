using FallingSloth;
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
            volumeSlider.value = SaveDataManager<OrbitalSaveData>.data.volume;
        }

        public void ChangeGlobalVolume(float volume)
        {
            Debug.Log("Changing global volume!");

            AudioManager.Instance.ChangeGlobalVolume(volume);

            SaveDataManager<OrbitalSaveData>.data.volume = volume;
            SaveDataManager<OrbitalSaveData>.SaveData();
        }

        public void ChangeGameColour(float hue)
        {
            Color colour = Color.HSVToRGB(hue, 0.73f, 1f);

            SaveDataManager<OrbitalSaveData>.data.colour = colour;
            SaveDataManager<OrbitalSaveData>.SaveData();

            ColourLoader.UpdateAllColours();
        }
    }
}