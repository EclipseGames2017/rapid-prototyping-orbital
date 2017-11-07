﻿using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class Particle : MonoBehaviour
    {
        [HideInInspector]
        new public Rigidbody2D rigidbody2D;

        public float downforceWhenDead = 10f;

        public ParticleDeathEffect deathEffectPrefab;
        static Pool<ParticleDeathEffect> deathEffectPool;

        bool isDead = false;

        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<ParticleDeathEffect>(deathEffectPrefab, 1);
            }
        }

        void OnEnable()
        {
            isDead = false;
        }

        void FixedUpdate()
        {
            if (!isDead) return;

            rigidbody2D.AddForce(Vector2.down * downforceWhenDead * Time.fixedDeltaTime);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "LauncherPad":
                    Destroy();
                    break;
                case "Nucleus":
                    isDead = true;
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Nucleus":
                    // TODO: Start orbiting the nucleus
                    break;
            }
        }

        void Destroy()
        {
            AudioManager.PlaySound("ExplosionSound");

            ParticleDeathEffect temp = deathEffectPool.GetObject();
            temp.transform.position = transform.position;
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);

            GameManager.ParticleDied();
        }
    }
}