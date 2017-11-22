using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    /// <summary>
    /// A singleton monobehaviour that manages core parts of the game.
    /// </summary>
    public class GameManager : SingletonBehaviour<GameManager>
    {
        /// <summary>
        /// The current state of the game.
        /// </summary>
        public static GameStates gameState { get; protected set; }

        /// <summary>
        /// A reference to the launcher object.
        /// </summary>
        public Launcher launcher;

        /// <summary>
        /// The particle prefab to use.
        /// </summary>
        public Ball ballPrefab;

        /// <summary>
        /// A pool of particles so that they can be reused.
        /// </summary>
        public static Pool<Ball> ballPool;

        /// <summary>
        /// The number of particles currently available to the player.
        /// </summary>
        public int maxBalls = 1;

        /// <summary>
        /// The number of particles that have been fired and subsequently been destroyed.
        /// </summary>
        static int deadBalls = 0;

        /// <summary>
        /// When firing multiple balls, there will be a delay of this many seconds between each ball being fired.
        /// </summary>
        public float delayBetweenBalls = .2f;

        /// <summary>
        /// The prefab for the things that will be orbited.
        /// </summary>
        public Target targetPrefab;

        /// <summary>
        /// A pool of nuclei that can be reused.
        /// </summary>
        public static Pool<Target> targetPool;

        public float g = (float)6.674e-11;
        public static float G { get { return Instance.g; } }

        protected override void Awake()
        {
            base.Awake();

            ballPool = new Pool<Ball>(ballPrefab, maxBalls);
            targetPool = new Pool<Target>(targetPrefab, 0);

            gameState = GameStates.FireBalls;
        }

        void Start()
        {
            Target temp = targetPool.GetObject();
            temp.transform.position = new Vector3(2f, 2f, 1f);
            temp.gameObject.SetActive(true);

            temp = targetPool.GetObject();
            temp.transform.position = new Vector3(0f, 0f, 1f);
            temp.gameObject.SetActive(true);

            temp = targetPool.GetObject();
            temp.transform.position = new Vector3(-2f, 2f, 1f);
            temp.gameObject.SetActive(true);
        }

        public static IEnumerator FireParticles(Vector2 direction, float magnitude)
        {
            gameState = GameStates.WaitForBalls;

            deadBalls = 0;

            for (int i = 0; i < Instance.maxBalls; i++)
            {
                AudioManager.PlaySound("ShootSound");

                Ball temp = ballPool.GetObject();
                temp.transform.position = Instance.launcher.transform.position;
                temp.maxVelocityMagnitude = magnitude;
                temp.gameObject.SetActive(true);
                temp.rigidbody2D.AddForce(direction * magnitude, ForceMode2D.Impulse);

                if (i < Instance.maxBalls - 1)
                    yield return new WaitForSeconds(Instance.delayBetweenBalls);
            }
        }

        public static void ParticleDied()
        {
            deadBalls++;

            if (deadBalls == Instance.maxBalls)
            {
                gameState = GameStates.SpawnNew;
                Instance.MoveOldNucleiDown();
            }
        }

        void MoveOldNucleiDown()
        {
            // TODO: Move all nuclei down one space

            // TODO: Check if any nucleus is on the bottom row
            // TODO: Define where the bottom row is

            // if (!loseConditionMet)
            gameState = GameStates.FireBalls;
            // else
            //     gameState = GameStates.GameOver;
        }
    }
}