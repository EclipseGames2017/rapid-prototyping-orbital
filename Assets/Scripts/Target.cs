using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FallingSloth;

namespace EclipseStudios.Orbital
{
    public class Target : MonoBehaviour
    {
        public int requiredOrbits = 1;

        public DeathEffect deathEffectPrefab;
        static Pool<DeathEffect> deathEffectPool;

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

        void Destroy()
        {
            DeathEffect temp = deathEffectPool.GetObject();
            temp.transform.position = transform.position;
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}