using FallingSloth;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        public static GameStates gameState { get; protected set; }

        public Particle particlePrefab;
        public static Pool<Particle> particles;
        public static int maxParticles = 1;

        public Nucleus nucleusPrefab;
        public static Pool<Nucleus> nuclei;

        protected override void Awake()
        {
            base.Awake();

            particles = new Pool<Particle>(particlePrefab, maxParticles);

            gameState = GameStates.FireBalls;
        }

        public static void FireParticles(Vector2 direction, float magnitude)
        {
            throw new System.NotImplementedException();
        }
    }
}