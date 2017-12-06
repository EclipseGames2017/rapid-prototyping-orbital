using FallingSloth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class GlowColourLoader : MonoBehaviour
    {
        public static List<GlowColourLoader> activeInstances;

        Outline outline;

        void Start()
        {
            outline = GetComponent<Outline>();
            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
        }

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<GlowColourLoader>();
            activeInstances.Add(this);
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
        }

        public void UpdateColour(Color colour)
        {
            outline.effectColor = colour;
        }
    }
}
