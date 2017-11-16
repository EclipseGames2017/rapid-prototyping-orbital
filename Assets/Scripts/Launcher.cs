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

        /// <summary>
        /// An array of sprite renderers used for the arrows coming out from the launcher.
        /// </summary>
        SpriteRenderer[] arrowRenderers;

        /// <summary>
        /// Whether or not the mouse button is currently being held down.
        /// </summary>
        bool isMouseButtonDown = false;

        /// <summary>
        /// The direction the balls will be fired in.
        /// </summary>
        Vector2 direction;

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

        void Start()
        {
            // Get the renderers for the launcher itself as well as all children.
            mainRenderer = GetComponent<SpriteRenderer>();
            arrowRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        }

        void Update()
        {
            // The player should only be able to fire if the game state says so.
            if (GameManager.gameState == GameStates.FireBalls)
            {
                // If the player can fire, they should be shown the thing they're firing from.
                mainRenderer.enabled = true;

                // If the mouse button was pressed this frame, check if it hit this object.
                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward));

                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        // Set this so we know if the button is still being held.
                        isMouseButtonDown = true;
                        // Enable all arrow renderers.
                        EnableArrowRenderers();
                    }
                }
                // Otherwise, if the mouse button was released this frame...
                else if (isMouseButtonDown && Input.GetMouseButtonUp(0))
                {
                    // Set this so we now the button's not being held any more.
                    isMouseButtonDown = false;
                    // Disable all arrow renderers.
                    DisableArrowRenderers();

                    // Calculate the angle to fire at.
                    float angle = Vector2.Angle(Vector2.up, direction);

                    // If the angle is withing the acceptable range, fire.
                    if (angle <= maxAngle)
                    {
                        // Clamp the magnitude.
                        float magnitude = Mathf.Clamp(direction.magnitude, minMagnitude, maxMagnitude);

                        // Fire the particles.
                        StartCoroutine(GameManager.FireParticles(direction.normalized, magnitude * forceMultiplier));
                    }
                }
            }
            // If the player is not allowed to fire, hide the launcher.
            else
            {
                mainRenderer.enabled = false;
            }

            //
            if (isMouseButtonDown)
            {
                // Calculate the rotation for the launcher.
                Vector2 launcherPosition = transform.position;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                direction = launcherPosition - mousePosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);

                // Calculate the angle of the trajectory.
                float fireAngle = Vector2.Angle(Vector2.up, direction);

                // Enable or disable arrow renderers based on the current angle.
                if (fireAngle <= maxAngle)
                    EnableArrowRenderers();
                else
                    DisableArrowRenderers();
            }
        }

        /// <summary>
        /// Gets the distance between the launcher and the mouse position.
        /// </summary>
        /// <returns>A direction vector from the launcher to the mouse position.</returns>
        Vector2 GetShotDirection()
        {
            Vector2 launcherPosition = transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            return (launcherPosition - mousePosition);
        }

        /// <summary>
        /// Enables all arrow renderers.
        /// </summary>
        void EnableArrowRenderers()
        {
            foreach (SpriteRenderer sr in arrowRenderers)
                sr.enabled = true;
        }

        /// <summary>
        /// Disables all arrow renderers.
        /// </summary>
        void DisableArrowRenderers()
        {
            foreach (SpriteRenderer sr in arrowRenderers)
                sr.enabled = false;
        }
    }
}