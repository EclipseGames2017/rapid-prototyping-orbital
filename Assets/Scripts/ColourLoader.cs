using FallingSloth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class ColourLoader : MonoBehaviour
    {
        public ColourObjectTypes type;

        [Range(0f, 1f)]
        public float alphaMultiplier = 1f;

        Object componentToColour;

        static List<ColourLoader> activeobjects;

        void Awake()
        {
            switch (type)
            {
                case ColourObjectTypes.Light:
                    componentToColour = GetComponent<Light>();
                    break;
                case ColourObjectTypes.ParticleSystem:
                    componentToColour = GetComponent<ParticleSystem>();
                    break;
                case ColourObjectTypes.Sprite:
                    componentToColour = GetComponent<SpriteRenderer>();
                    break;
                case ColourObjectTypes.UI_Image:
                    componentToColour = GetComponent<Image>();
                    break;
                case ColourObjectTypes.UI_Outline:
                    componentToColour = GetComponent<Outline>();
                    break;
            }
        }

        void OnEnable()
        {
            if (activeobjects == null)
                activeobjects = new List<ColourLoader>();
            activeobjects.Add(this);

            UpdateColour(SaveDataManager<OrbitalSaveData>.data.colour);

            if (type == ColourObjectTypes.ParticleSystem)
            {
                ParticleSystem.EmissionModule temp = (componentToColour as ParticleSystem).emission;
                temp.enabled = true;
            }
        }
        void OnDisable()
        {
            activeobjects.Remove(this);

            if (type == ColourObjectTypes.ParticleSystem)
            {
                ParticleSystem.EmissionModule temp = (componentToColour as ParticleSystem).emission;
                temp.enabled = false;
            }
        }

        void UpdateColour(Color colour)
        {
            colour.a *= alphaMultiplier;
            switch (type)
            {
                case ColourObjectTypes.Light:
                    (componentToColour as Light).color = colour;
                    break;
                case ColourObjectTypes.ParticleSystem:
                    ParticleSystem.MainModule temp = (componentToColour as ParticleSystem).main;
                    temp.startColor = colour;
                    break;
                case ColourObjectTypes.Sprite:
                    (componentToColour as SpriteRenderer).color = colour;
                    break;
                case ColourObjectTypes.UI_Image:
                    (componentToColour as Image).color = colour;
                    break;
                case ColourObjectTypes.UI_Outline:
                    (componentToColour as Outline).effectColor = colour;
                    break;
            }
        }

        public static void UpdateAllColours()
        {
            if (activeobjects == null) return;

            Color colour = SaveDataManager<OrbitalSaveData>.data.colour;
            activeobjects.ForEach(obj => obj.UpdateColour(colour));
        }

        public void FlashWhite()
        {
            StopCoroutine("_FlashWhite");
            StartCoroutine(_FlashWhite(4f));
        }
        IEnumerator _FlashWhite(float speed, int iterations = 2)
        {
            Color startColour = SaveDataManager<OrbitalSaveData>.data.colour;
            for (int i = 0; i < iterations; i++)
            {
                for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime * speed)
                {
                    UpdateColour(Color.Lerp(startColour, Color.white, t));
                    yield return null;
                }
                for (float t = 1.0f; t >= 0.0f; t -= Time.deltaTime * speed)
                {
                    UpdateColour(Color.Lerp(startColour, Color.white, t));
                    yield return null;
                }
            }
            UpdateColour(startColour);
        }

        public enum ColourObjectTypes
        {
            UI_Image,
            UI_Outline,
            Sprite,
            Light,
            ParticleSystem
        }
    }
}