using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FallingSloth;
using FallingSloth.Audio;

namespace EclipseStudios.Orbital
{
    public class Target : MonoBehaviour
    {
        public float requiredOrbits = 10;
        public float newOrbitValue = 10;

        public DeathEffect deathEffectPrefab;
        public static Pool<DeathEffect> deathEffectPool;
        public TextMesh targetText;

        [HideInInspector]
        new public Rigidbody2D rigidbody2D;

        new Light light;
        SpriteRenderer outline;

        public float colourLerpSpeed = 1f;

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            light = GetComponent<Light>();
            outline = transform.Find("Outline").GetComponent<SpriteRenderer>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<DeathEffect>(deathEffectPrefab, 1);
            }
        }

        void OnEnable()
        {
            if (light != null)
                light.color = SaveDataManager<OrbitalSaveData>.data.colour;
            if (outline != null)
                outline.color = SaveDataManager<OrbitalSaveData>.data.colour;
        }

        void Update()
        {
            if (requiredOrbits <= 0.0f)
                Destroy();

            targetText.text = Mathf.Ceil(requiredOrbits).ToString("F0");
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (gameObject.activeSelf)
            {
                switch (collision.tag)
                {
                    case "Ball":
                        StartCoroutine(LerpToWhite());
                        break;
                }
            }
        }
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (gameObject.activeSelf)
            {
                switch (collision.gameObject.tag)
                {
                    case "Ball":
                        StartCoroutine(LerpFromWhite());
                        break;
                }
            }
        }
        void OnTriggerExit2D(Collider2D collision)
        {
            if (gameObject.activeSelf)
            {
                switch (collision.tag)
                {
                    case "Ball":
                        StartCoroutine(LerpFromWhite());
                        break;
                }
            }
        }

        // Destroys target and will increase between a range
        void Destroy()
        {
            AudioManager.PlaySound("target_destroy");
            DeathEffect temp = deathEffectPool.GetObject();
            temp.transform.position = new Vector3(transform.position.x, transform.position.y, temp.transform.position.z);
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);
            newOrbitValue += Random.value < 0.5 ? 10 : 20 < 0.9 ? 10 : 30;
            requiredOrbits = newOrbitValue;
        }

        IEnumerator LerpToWhite()
        {
            Color startColour = SaveDataManager<OrbitalSaveData>.data.colour;
            for (float i = 0f; i <= 1f; i += Time.deltaTime * colourLerpSpeed)
            {
                Color c = Color.Lerp(startColour, Color.white, i);
                light.color = c;
                outline.color = c;
                yield return null;
            }
        }
        IEnumerator LerpFromWhite()
        {
            Color targetColour = SaveDataManager<OrbitalSaveData>.data.colour;
            for (float i = 0f; i <= 1f; i += Time.deltaTime * colourLerpSpeed)
            {
                Color c = Color.Lerp(Color.white, targetColour, i);
                light.color = c;
                outline.color = c;
                yield return null;
            }
        }
    }
}