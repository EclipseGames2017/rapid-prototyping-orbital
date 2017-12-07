using FallingSloth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class ParticleColourLoader : MonoBehaviour
    {
        public static List<ParticleColourLoader> activeInstances;

        ParticleSystem particles;

        public Gradient defaultGradient;

        void Start()
        {
            particles = GetComponent<ParticleSystem>();
            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
        }

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<ParticleColourLoader>();
            activeInstances.Add(this);
            particles.enableEmission = false;
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
        }

        public void UpdateColour(Color colour)
        {
            System.Array.ForEach(defaultGradient.colorKeys, (gc) => {
                gc.color *= colour;
                Debug.Log("Colour set to: " + gc.color);
            });

            ParticleSystem.MainModule main = particles.main;
            main.startColor = defaultGradient;
        }
    }
}
