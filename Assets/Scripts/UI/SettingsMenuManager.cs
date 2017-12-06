﻿using FallingSloth;
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

            if (GlowColourLoader.activeInstances != null)
                foreach (GlowColourLoader obj in GlowColourLoader.activeInstances)
                    obj.UpdateColour(colour);

            if (ImageColourLoader.activeInstances != null)
                foreach (ImageColourLoader obj in ImageColourLoader.activeInstances)
                    obj.UpdateColour(colour);

            if (SpriteColourLoader.activeInstances != null)
                foreach (SpriteColourLoader obj in SpriteColourLoader.activeInstances)
                    obj.UpdateColour(colour);

            if (LightColourLoader.activeInstances != null)
                foreach (LightColourLoader obj in LightColourLoader.activeInstances)
                    obj.UpdateColour(colour);

            SaveDataManager<OrbitalSaveData>.data.colour = colour;
            SaveDataManager<OrbitalSaveData>.SaveData();
        }
    }
}