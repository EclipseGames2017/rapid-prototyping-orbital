using FallingSloth;
using FallingSloth.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace EclipseStudios.Orbital
{
    /// <summary>
    /// A singleton monobehaviour that manages core parts of the game.
    /// </summary>
    public class GameManager : SingletonBehaviour<GameManager>
    {
        #if UNITY_ANDROID
        const string GAMEID = "1617456";
        #elif UNITY_IOS
        const string GAMEID = "1617457";
        #endif

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

        public int maxTargets = 10;

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

        [Range(1, 5)]
        public int maxObjectToSpawnEachTurn = 3;

        public float powerupSpawnChance = .1f;

        int turnCount = 0;

        public bool hasContinued = false;

        [HideInInspector]
        public bool isPaused = false;
        public Button pauseButton;
        public Animator pauseMenu;

        protected override void Awake()
        {
            base.Awake();

#if UNITY_ANDROID || UNITY_IOS
            // Initialize the ads system
            Advertisement.Initialize(GAMEID);
#endif

            ballPool = new Pool<Ball>(ballPrefab, maxBalls);
            targetPool = new Pool<Target>(targetPrefab, maxTargets, false);
            powerupPool = new Pool<BallPowerup>(powerupPrefab, 5);

            gameState = GameStates.SpawnNew;
            SpawnNewStuff();
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
            List<int> validPositions = new List<int> { -2, -1, 0, 1, 2 };
            for (int i = 0; i < Random.Range(1, maxObjectToSpawnEachTurn+1); i++)
            {
                GameObject temp;
                if (Random.value > powerupSpawnChance)
                {
                    try { temp = targetPool.GetObject().gameObject; }
                    catch { continue; }
                }
                else
                    temp = powerupPool.GetObject().gameObject;

                int randomPosition = Random.Range(0, validPositions.Count);
                temp.transform.position = new Vector3(validPositions[randomPosition], 6f, temp.transform.position.z);
                validPositions.RemoveAt(randomPosition);

                temp.SetActive(true);
            }

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
            
            Vector3[] startPos = new Vector3[objectsToMove.Count];
            Vector3[] endPos = new Vector3[objectsToMove.Count];

            for (int i = 0; i < objectsToMove.Count; i++)
            {
                startPos[i] = objectsToMove[i].transform.position;
                if (startPos[i].y > 5f)
                    endPos[i] = new Vector3(startPos[i].x, 4f, startPos[i].z);
                else
                    endPos[i] = new Vector3(startPos[i].x, startPos[i].y - 1f, startPos[i].z);
            }

            for (float t = 0f; t <= 1f; t += Time.deltaTime * objectMoveSpeed)
            {
                for (int i = 0; i < objectsToMove.Count; i++)
                {
                    objectsToMove[i].transform.position = Vector3.Lerp(startPos[i], endPos[i], t);
                }
                yield return null;
            }
            
            for (int i = 0; i < objectsToMove.Count; i++)
            {
                objectsToMove[i].transform.position = endPos[i].Round();

                // If the thing is at the end and that thing is a target, then the lose condition has been met
                if (objectsToMove[i].transform.position.y <= -3f)
                {
                    switch (objectsToMove[i].tag)
                    {
                        case "Target":
                            AudioManager.PlaySound("game_over_sound");
                            gameState = GameStates.GameOver;
                            Debug.Log("Showing continue screen...");
                            ContinueScreen.Show();
                            yield break;
                        case "Powerup":
                            objectsToMove[i].GetComponent<BallPowerup>().Destroy();
                            break;
                    }
                }
            }

            // If we're here, the lose condition hasn't been met, continue play.
            gameState = GameStates.FireBalls;
        }

        public static void PlayAgain()
        {
            Debug.Log("Preparing for next round...");

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public static void WatchAdToContinue()
        {
#if UNITY_ANDROID || UNITY_IOS
            ShowOptions options = new ShowOptions { resultCallback = Instance.ShowAdCallback };
            Advertisement.Show(options);
#else
            Debug.Log("Unsupported platform for ads!");
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        void ShowAdCallback(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Failed:
                    Debug.LogError("Ad failed to show!");
                    PlayAgain();
                    break;
                case ShowResult.Skipped:
                    Debug.LogWarning("Bastard skipped the ad!");
                    PlayAgain();
                    break;
                case ShowResult.Finished:
                    Debug.Log("Ad complete!");
                    ContinueGame();
                    break;
            }
        }
#endif

        void ContinueGame()
        {
            hasContinued = true;

            // Get a list of all the things that need to be removed.
            List<GameObject> objectsInLevel = new List<GameObject>();
            objectsInLevel.AddRange(targetPool.GetActiveObjects().Select(obj => obj.gameObject));
            objectsInLevel.AddRange(powerupPool.GetActiveObjects().Select(obj => obj.gameObject));

            objectsInLevel.ForEach(obj => { if (obj.transform.position.y <= -2f) { obj.SendMessage("Destroy"); } });

            gameState = GameStates.FireBalls;
        }

        public void PauseOrResume()
        {
            isPaused = !isPaused;
            pauseButton.gameObject.SetActive(!isPaused);
            pauseMenu.SetTrigger(isPaused ? "Open" : "Close");
            Time.timeScale = (isPaused) ? 0f : 1f;
        }
    }
}