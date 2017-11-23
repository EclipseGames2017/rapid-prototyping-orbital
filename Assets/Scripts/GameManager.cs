using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// The prefab for the things that will be orbited.
        /// </summary>
        public Target targetPrefab;

        /// <summary>
        /// A pool of nuclei that can be reused.
        /// </summary>
        public static Pool<Target> targetPool;

        public BallPowerup powerupPrefab;
        public static Pool<BallPowerup> powerupPool;

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

        public float g = (float)6.674e-11;
        public static float G { get { return Instance.g; } }

        public float objectMoveSpeed = 5f;

        int turnCount = 0;

        protected override void Awake()
        {
            base.Awake();

            ballPool = new Pool<Ball>(ballPrefab, maxBalls);
            targetPool = new Pool<Target>(targetPrefab, 5);
            powerupPool = new Pool<BallPowerup>(powerupPrefab, 5);

            gameState = GameStates.FireBalls;
        }

        void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                Target temp = targetPool.GetObject();
                temp.transform.position = new Vector3(Random.Range(-2, 3), 4-i, 4);
                temp.gameObject.SetActive(true);
            }
        }

        public static IEnumerator FireParticles(Vector2 direction, float magnitude)
        {
            Instance.turnCount++;

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
                Instance.SpawnNewStuff();
            }
        }

        void SpawnNewStuff()
        {
            //TODO: Spawn new stuff

            StartCoroutine(MoveStuffDown());
        }

        IEnumerator MoveStuffDown()
        {
            // Get a list of all the things that need to be moved down.
            List<GameObject> objectsToMove = new List<GameObject>();
            objectsToMove.AddRange(targetPool.GetActiveObjects().Select(obj => obj.gameObject));
            objectsToMove.AddRange(powerupPool.GetActiveObjects().Select(obj => obj.gameObject));

            // Sort the list by y position: lowest first
            objectsToMove.Sort((GameObject obj1, GameObject obj2) => { return obj1.transform.position.y.CompareTo(obj2.transform.position.y); });

            for (int i = 0; i < objectsToMove.Count; i++)
            {
                Vector3 startPos = objectsToMove[i].transform.position;
                Vector3 endPos;
                if (startPos.y > 5f)
                    endPos = new Vector3(startPos.x, 4f, startPos.z);
                else
                    endPos = new Vector3(startPos.x, startPos.y - 1f, startPos.z);

                for (float t = 0f; t <= 1f; t += Time.deltaTime*objectMoveSpeed)
                {
                    objectsToMove[i].transform.position = Vector3.Lerp(startPos, endPos, t);
                    yield return null;
                }
                objectsToMove[i].transform.position = endPos;
            }

            // if (!loseConditionMet)
                gameState = GameStates.FireBalls;
            // else
            //     gameState = GameStates.GameOver;
        }
    }
}