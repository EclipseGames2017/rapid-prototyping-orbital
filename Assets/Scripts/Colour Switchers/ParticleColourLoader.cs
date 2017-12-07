using FallingSloth;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class ParticleColourLoader : MonoBehaviour
    {
        public static List<ParticleColourLoader> activeInstances;

        new ParticleSystem particleSystem;
        ParticleSystem.MainModule particlesMain;
        ParticleSystem.EmissionModule particlesEmission;

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<ParticleColourLoader>();
            activeInstances.Add(this);

            if (particleSystem == null)
            {
                particleSystem = GetComponent<ParticleSystem>();
                particlesMain = particleSystem.main;
                particlesEmission = particleSystem.emission;
            }

            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
            particlesEmission.enabled = true;
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
            particlesEmission.enabled = false;
        }

        public void UpdateColour(Color colour)
        {
            colour.a = 0.5f;
            particlesMain.startColor = colour;
        }
    }
}
