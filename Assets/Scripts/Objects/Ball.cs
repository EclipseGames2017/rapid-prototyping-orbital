using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    /// <summary>
    /// A ball fired by the player.
    /// </summary>
    public class Ball : MonoBehaviour
    {
        /// <summary>
        /// The rigidbody attached to this ball instance.
        /// </summary>
        [HideInInspector]
        new public Rigidbody2D rigidbody2D;
        
        /// <summary>
        /// The multiplier for how fast the ball takes down targets.
        /// </summary>
        public float ballPower;
        float ballPowerLastFrame = 0;

        /// <summary>
        /// 
        /// </summary>
        public float PowerIncrease;

        /// <summary>
        /// The trail renderer attached to this ball instance.
        /// </summary>
        TrailRenderer trailRenderer;

        /// <summary>
        /// The downward force applied to the ball each frame.
        /// </summary>
        public float downForce = 1f;

        /// <summary>
        /// The maximum magnitude for the balls velocity.  Set when firing the ball.
        /// </summary>
        public float maxVelocityMagnitude = 2f;

        /// <summary>
        /// 
        /// </summary>
        public float maxGravityDistance = 2;

        /// <summary>
        /// The prefab to use for ball death effects.
        /// </summary>
        public DeathEffect deathEffectPrefab;

        /// <summary>
        /// A pool of the deathEffectPrefab.
        /// </summary>
        public static Pool<DeathEffect> deathEffectPool;

        /// <summary>
        /// The balls velocity will be multiplied by this value when entering a target's trigger, and divided by it when leaving.
        /// </summary>
        public float velocityReductionMultiplier = .85f;

        int bounceCount = 0;

        public int maxBounces = 10;

        public float superDownForce = 100f;

        public ScoreManager scoremanager;

        public Text ballPowerText;
        ColourLoader ballPowerTextColour;

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
            ballPowerTextColour = ballPowerText.GetComponent<ColourLoader>();

            rigidbody2D = GetComponent<Rigidbody2D>();

            if (deathEffectPool == null)
            {
                deathEffectPool = new Pool<DeathEffect>(deathEffectPrefab, 1);
            }
        }

        void Start()
        {
            ballPowerText.text = "X 1";
        }
        void OnEnable()
        {
            trailRenderer.enabled = true;
            isDead = false;
            bounceCount = 0;
        }
        void OnDisable()
        {
            trailRenderer.enabled = false;
            trailRenderer.Clear();
        }

        // once the player earns any score, the text will be updated to show the initials of each text
        void Update()
        {
            if (ballPower > ballPowerLastFrame)
            {
                ballPowerText.text = "X " + ballPower.ToString("F0");
                ballPowerTextColour.FlashWhite();
                ballPowerLastFrame = ballPower;
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

            if (bounceCount <= maxBounces)
                v += Vector2.down * downForce;
            else
                v += Vector2.down * superDownForce;

            rigidbody2D.velocity = Vector2.ClampMagnitude(v, maxVelocityMagnitude);
        }

        // if the ball gets destroyed, reset the ball multiplier and destroy the ball
        void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "wall":
                    AudioManager.PlaySound("wall_bounce");
                    bounceCount++;
                    break;

                case "LauncherPad":
                case "Target":
                    scoremanager.multiplier = 0;
                    Destroy();
                    break;
            }
        }

        // Entering the target gravity field will increase points and reduce velocity of ball
        void OnTriggerEnter2D(Collider2D collider)
        {
            switch (collider.gameObject.tag)
            {
                case "Target":
                    AudioManager.PlaySound("increase_multiplier");
                    AudioManager.PlaySound("target_orbit");
                    rigidbody2D.velocity *= velocityReductionMultiplier;
                    scoremanager.multiplier++;
                    bounceCount = 0;
                    break;
            }
            // Colliding with a powerup means the ball power will increase and will destroy the powerup
            if (collider.gameObject.tag == "Powerup")
            {
                AudioManager.PlaySound("pickup_powerup");
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
                collider.GetComponent<Target>().requiredOrbits -= ballPower * Time.deltaTime * 10;
                scoremanager.currentScore += scoremanager.multiplier * Time.deltaTime * 2;
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
            temp.transform.position = new Vector3(transform.position.x, transform.position.y, temp.transform.position.z);
            temp.gameObject.SetActive(true);
            gameObject.SetActive(false);

            GameManager.ParticleDied();
        }
    }
}