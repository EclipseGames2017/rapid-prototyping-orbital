using FallingSloth;
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
        public static int maxParticles = 3;

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
            //gameState = GameStates.WaitForBalls;

            for (int i = 0; i < maxParticles; i++)
            {
                Particle temp = particles.GetObject();
                temp.transform.position = Instance.launcher.transform.position;
                temp.gameObject.SetActive(true);

                temp.rigidbody2D.AddForce(direction * magnitude, ForceMode2D.Impulse);

                if (i < maxParticles - 1)
                    yield return new WaitForSeconds(Instance.delayBetweenParticles);
            }
        }
    }
}