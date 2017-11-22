using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class Ball : MonoBehaviour
    {
        [HideInInspector]
        new public Rigidbody2D rigidbody2D;

        public float downForce = 1f;

        public float maxVelocityMagnitude = 2f;
        public float maxGravityDistance = 2;

        public DeathEffect deathEffectPrefab;
        static Pool<DeathEffect> deathEffectPool;

        public float velocityReductionMultiplier = .85f;

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
            rigidbody2D = GetComponent<Rigidbody2D>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<DeathEffect>(deathEffectPrefab, 1);
            }
        }

        void OnEnable()
        {
            isDead = false;
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

        void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "LauncherPad":
                case "Target":
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
                    break;
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