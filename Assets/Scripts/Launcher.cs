using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class Launcher : MonoBehaviour
    {
        SpriteRenderer mainRenderer;
        SpriteRenderer[] arrowRenderers;

        bool isMouseButtonDown = false;

        Vector2 direction;

        public float minMagnitude = 0.2f;
        public float maxMagnitude = 2f;

        public float forceMultiplier = 10f;

        void Start()
        {
            mainRenderer = GetComponent<SpriteRenderer>();
            arrowRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
        }

        void Update()
        {
            if (GameManager.gameState == GameStates.FireBalls)
            {
                mainRenderer.enabled = true;

                if (Input.GetMouseButtonDown(0))
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward));

                    if (hit.collider != null && hit.collider.gameObject == this.gameObject)
                    {
                        isMouseButtonDown = true;
                        EnableArrowRenderers();
                    }
                }
                else if (isMouseButtonDown && Input.GetMouseButtonUp(0))
                {
                    isMouseButtonDown = false;
                    DisableArrowRenderers();

                    float magnitude = Mathf.Clamp(direction.magnitude, minMagnitude, maxMagnitude);

                    if (direction.magnitude >= minMagnitude)
                        StartCoroutine(GameManager.FireParticles(direction.normalized, magnitude * forceMultiplier));
                }
            }
            else
            {
                mainRenderer.enabled = false;
            }

            if (isMouseButtonDown)
            {
                Vector2 launcherPosition = transform.position;
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                direction = launcherPosition - mousePosition;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        Vector2 GetShotDirection()
        {
            Vector2 launcherPosition = transform.position;
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            return (launcherPosition - mousePosition);
        }

        void EnableArrowRenderers()
        {
            foreach (SpriteRenderer sr in arrowRenderers)
                sr.enabled = true;
        }
        void DisableArrowRenderers()
        {
            foreach (SpriteRenderer sr in arrowRenderers)
                sr.enabled = false;
        }
    }
}