using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    /// <summary>
    /// Enables the player to launch a ball.
    /// </summary>
    public class Launcher : MonoBehaviour
    {
        /// <summary>
        /// The sprite renderer for the launcher.
        /// </summary>
        SpriteRenderer mainRenderer;

        public LineRenderer trajectoryLine;

        /// <summary>
        /// The direction the balls will be fired in.
        /// </summary>
        Vector2 direction;
        float angle;
        float shotMagnitude;

        /// <summary>
        /// The minimum magnitude for the direction vector.
        /// </summary>
        public float minMagnitude = 0.2f;

        /// <summary>
        /// The maximum magnitude for the direction vector.
        /// </summary>
        public float maxMagnitude = 2f;

        /// <summary>
        /// A multiplier for the force that is applied to the ball.
        /// </summary>
        public float forceMultiplier = 10f;

        /// <summary>
        /// The maximum angle the ball can be fired at.  This is the angle between Vector2.up and the direction vector.
        /// </summary>
        public float maxAngle = 85f;

        int fingerID = -1;
        Vector2 touchStartPosition = -Vector2.one;

        void Start()
        {
            // Get the renderers for the launcher itself.
            mainRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            // The player should only be able to fire if the game state says so.
            if (GameManager.gameState == GameStates.FireBalls)
            {
                // If the player can fire, they should be shown the thing they're firing from.
                mainRenderer.enabled = true;

                #region Touch Input
                #if UNITY_ANDROID || UNITY_IOS
                // If no touch has been caught yet, check for one
                if (fingerID == -1)
                {
                    foreach (Touch t in Input.touches)
                    {
                        if (t.phase == TouchPhase.Began)
                        {
                            Debug.Log("Touch began!");
                            fingerID = t.fingerId;
                            touchStartPosition = Camera.main.ScreenToWorldPoint(t.position);
                            break;
                        }
                    }
                }
                else if (fingerID >= 0)
                {
                    foreach (Touch t in Input.touches)
                    {
                        if (t.fingerId == fingerID)
                        {
                            switch (t.phase)
                            {
                                case TouchPhase.Canceled:
                                    fingerID = -1;
                                    touchStartPosition = -Vector2.one;
                                    break;
                                case TouchPhase.Moved:
                                    SetDirectionAndAngle(touchStartPosition, Camera.main.ScreenToWorldPoint(t.position));
                                    break;
                                case TouchPhase.Ended:
                                    SetDirectionAndAngle(touchStartPosition, Camera.main.ScreenToWorldPoint(t.position));
                                    Fire();
                                    break;
                            }
                        }
                    }
                }
#endif
                #endregion
                #region Mouse Input
                #if !(UNITY_ANDROID || UNITY_IOS) || UNITY_EDITOR
                if (fingerID == -1)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        fingerID = -2;
                        touchStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    }
                }
                else if (fingerID == -2)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        SetDirectionAndAngle(touchStartPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                        Fire();
                    }
                    else if (Input.GetMouseButton(0))
                    {
                        SetDirectionAndAngle(touchStartPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                    else
                    {
                        fingerID = -1;
                        touchStartPosition = -Vector2.one;
                    }
                }
                #endif
                #endregion
            }
            // If the player is not allowed to fire, hide the launcher.
            else
            {
                mainRenderer.enabled = false;
                trajectoryLine.enabled = false;
            }
        }

        void SetDirectionAndAngle(Vector2 start, Vector2 end)
        {
            direction = (start - end);
            shotMagnitude = direction.magnitude;
            direction = direction.normalized;
            angle = Vector2.SignedAngle(Vector2.up, direction);

            if (Mathf.Abs(angle) <= maxAngle)
            {
                trajectoryLine.enabled = true;
                trajectoryLine.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
                trajectoryLine.enabled = false;
        }

        void Fire()
        {
            if (Mathf.Abs(angle) <= maxAngle)
                StartCoroutine(GameManager.FireParticles(direction, Mathf.Clamp(shotMagnitude * forceMultiplier, minMagnitude, maxMagnitude)));
            fingerID = -1;
            touchStartPosition = -Vector2.one;
        }
    }
}