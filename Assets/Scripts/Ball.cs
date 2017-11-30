﻿using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    public class Ball : MonoBehaviour
    {
        [HideInInspector]
        new public Rigidbody2D rigidbody2D;
        
        public float ballPower;
        public float PowerIncrease;

        TrailRenderer trailRenderer;

        public float downForce = 1f;

        public float maxVelocityMagnitude = 2f;
        public float maxGravityDistance = 2;

        public DeathEffect deathEffectPrefab;
        public static Pool<DeathEffect> deathEffectPool;

        public float velocityReductionMultiplier = .85f;

        public ScoreManager scoremanager;

        public Text ballPowerText;

        bool d = false;
        bool isDead
        {
            get { return d; }
            set
            {
                d = value;
            }
        }

        void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();

            scoremanager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            ballPowerText = GameObject.Find("BallPowerText").GetComponent<Text>();

            rigidbody2D = GetComponent<Rigidbody2D>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<DeathEffect>(deathEffectPrefab, 1);
            }
        }

        void Start()
        {
            ballPowerText.text = "Ball Power: 1";
        }
        void OnEnable()
        {
            trailRenderer.enabled = true;
            isDead = false;
        }
        void OnDisable()
        {
            trailRenderer.enabled = false;
        }

        // once the player earns any score, the text will be updated to show the initials of each text
        void Update()
        {
            if (scoremanager.currentScore >= 0.1)
            {
                ballPowerText.text = "BP: " + ballPower.ToString("F0");
            }
            
        }
        void FixedUpdate()
        {
            Target[] targets = GameManager.targetPool.GetActiveObjects();
            Vector2 v = rigidbody2D.velocity;
            foreach (Target target in targets)
            {
                float r = Vector2.Distance(this.transform.position, target.transform.position);

                if (r > maxGravityDistance)
                    continue;

                float m1 = target.rigidbody2D.mass;
                float m2 = this.rigidbody2D.mass;
                float F = GameManager.G * ((m1 * m2) / Mathf.Pow(r, 2));
                Vector2 direction = (target.transform.position - this.transform.position).normalized;
                v += (F * direction);
            }

            v += Vector2.down * downForce;

            rigidbody2D.velocity = Vector2.ClampMagnitude(v, maxVelocityMagnitude);
        }

        // if the ball gets destroyed, reset the ball multiplier and destroy the ball
        void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "LauncherPad":
                case "Target":
                    scoremanager.multiplier = 1;
                    Destroy();
                    break;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            switch (collider.gameObject.tag)
            {
                case "Target":
                    rigidbody2D.velocity *= velocityReductionMultiplier;
                    scoremanager.multiplier++;
                    
                    break;
            }
            // Colliding with a powerup means the ball power will increase and will destroy the powerup
            if (collider.gameObject.tag == "Powerup")
            {
                ballPower += PowerIncrease;
                collider.gameObject.GetComponent<BallPowerup>().Destroy();
            }
        }

        // When the ball reaches the target trigger area the ball will take value away from the target
        // Score will increase as the ball stays in the gravity radius of the target, being times by the multiplier
        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.tag == "Target")
            {
                collider.GetComponent<Target>().requiredOrbits -= ballPower * Time.deltaTime;
                scoremanager.currentScore += scoremanager.multiplier * Time.deltaTime;
            }
        }
        void OnTriggerExit2D(Collider2D collider)
        {
            switch (collider.gameObject.tag)
            {
                case "Target":
                    rigidbody2D.velocity /= velocityReductionMultiplier;
                    break;
            }
        }

        void Destroy()
        {
            AudioManager.PlaySound("ExplosionSound");

            DeathEffect temp = deathEffectPool.GetObject();
            temp.transform.position = transform.position;
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);

            GameManager.ParticleDied();
        }
    }
}