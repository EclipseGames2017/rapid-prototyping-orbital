using FallingSloth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class LightColourLoader : MonoBehaviour
    {
        public static List<LightColourLoader> activeInstances;

        new Light light;

        void Start()
        {
            light = GetComponent<Light>();
            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
        }

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<LightColourLoader>();
            activeInstances.Add(this);
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
        }

        public void UpdateColour(Color colour)
        {
            light.color = colour;
        }
    }
}
