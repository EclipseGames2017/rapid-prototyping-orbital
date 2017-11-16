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
        public Particle particlePrefab;
        /// <summary>
        /// A pool of particles so that they can be reused.
        /// </summary>
        public static Pool<Particle> particles;
        /// <summary>
        /// The number of particles currently available to the player.
        /// </summary>
        public int maxParticles = 1;
        /// <summary>
        /// The number of particles that have been fired and subsequently been destroyed.
        /// </summary>
        static int deadParticles = 0;

        /// <summary>
        /// When firing multiple balls, there will be a delay of this many seconds between each ball being fired.
        /// </summary>
        public float delayBetweenParticles = .2f;

        /// <summary>
        /// The prefab for the things that will be orbited.
        /// </summary>
        public Nucleus nucleusPrefab;
        /// <summary>
        /// A pool of nuclei that can be reused.
        /// </summary>
        public static Pool<Nucleus> nuclei;

        protected override void Awake()
        {
            base.Awake();

            particles = new Pool<Particle>(particlePrefab, maxParticles);

            gameState = GameStates.FireBalls;
        }

        public static IEnumerator FireParticles(Vector2 direction, float magnitude)
        {
            gameState = GameStates.WaitForBalls;

            deadParticles = 0;

            for (int i = 0; i < Instance.maxParticles; i++)
            {
                AudioManager.PlaySound("ShootSound");

                Particle temp = particles.GetObject();
                temp.transform.position = Instance.launcher.transform.position;
                temp.gameObject.SetActive(true);

                temp.rigidbody2D.AddForce(direction * magnitude, ForceMode2D.Impulse);

                if (i < Instance.maxParticles - 1)
                    yield return new WaitForSeconds(Instance.delayBetweenParticles);
            }
        }

        public static void ParticleDied()
        {
            deadParticles++;

            if (deadParticles == Instance.maxParticles)
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