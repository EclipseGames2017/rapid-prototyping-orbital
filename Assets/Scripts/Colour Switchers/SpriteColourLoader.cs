using FallingSloth;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class SpriteColourLoader : MonoBehaviour
    {
        public static List<SpriteColourLoader> activeInstances;

        SpriteRenderer sprite;

        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);
        }

        void OnEnable()
        {
            if (activeInstances == null)
                activeInstances = new List<SpriteColourLoader>();
            activeInstances.Add(this);
        }
        void OnDisable()
        {
            activeInstances.Remove(this);
        }

        public void UpdateColour(Color colour)
        {
            sprite.color = colour;
        }
    }
}
