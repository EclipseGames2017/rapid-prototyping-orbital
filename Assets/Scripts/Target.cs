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
        public float newOrbitValue = 1;

        public DeathEffect deathEffectPrefab;
        public static Pool<DeathEffect> deathEffectPool;
        public TextMesh targetText;

        [HideInInspector]
        new public Rigidbody2D rigidbody2D;

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<DeathEffect>(deathEffectPrefab, 1);
            }
        }

        void Update()
        {
            if (requiredOrbits <= 0f)
                Destroy();
            targetText.text = requiredOrbits.ToString("F1");

        }

        void Destroy()
        {
            AudioManager.PlaySound("target_destroy");
            DeathEffect temp = deathEffectPool.GetObject();
            temp.transform.position = transform.position;
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);
            newOrbitValue += Random.Range(1, 3);
            requiredOrbits = newOrbitValue;
        }
    }
}