using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FallingSloth;
using System.Collections.Generic;

namespace EclipseStudios.Orbital
{
    public class ImageColourLoader : MonoBehaviour
    {
        public static List<ImageColourLoader> activeInstances;

        Image image;

        void Start()
        {
            image = GetComponent<Image>();
            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
        }

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<ImageColourLoader>();
            activeInstances.Add(this);
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
        }

        public void UpdateColour(Color colour)
        {
            image.color = colour;
        }
    }
}