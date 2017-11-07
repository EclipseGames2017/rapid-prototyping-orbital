using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public static GameStates gameState { get; protected set; }

        public Launcher launcher;

        public Particle particlePrefab;
        public static Pool<Particle> particles;
        public static int maxParticles = 10;
        static int deadParticles = 0;

        public float delayBetweenParticles = .2f;

        public Nucleus nucleusPrefab;
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

            for (int i = 0; i < maxParticles; i++)
            {
                AudioManager.PlaySound("ShootSound");

                Particle temp = particles.GetObject();
                temp.transform.position = Instance.launcher.transform.position;
                temp.gameObject.SetActive(true);

                temp.rigidbody2D.AddForce(direction * magnitude, ForceMode2D.Impulse);

                if (i < maxParticles - 1)
                    yield return new WaitForSeconds(Instance.delayBetweenParticles);
            }
        }

        public static void ParticleDied()
        {
            deadParticles++;

            if (deadParticles == maxParticles)
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
        }
    }
}