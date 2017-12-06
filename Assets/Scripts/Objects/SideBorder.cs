using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    /// <summary>
    /// Sets up the borders for the screen.  These will have colliders attatched to contain the particles.
    /// </summary>
    public class SideBorder : MonoBehaviour
    {
        /// <summary>
        /// The side of the side the border is on.
        /// </summary>
        public BorderSides side;

        public float thickness = 2f;

        void Awake()
        {
            Camera cam = Camera.main;
            switch (side)
            {
                case BorderSides.Top:
                    transform.position = new Vector2(0f, cam.orthographicSize + (thickness / 2f));
                    transform.localScale = new Vector3(cam.orthographicSize * cam.aspect * 2.5f, thickness, 1f);
                    break;
                case BorderSides.Bottom:
                    transform.position = new Vector2(0f, -cam.orthographicSize - (thickness / 2f));
                    transform.localScale = new Vector3(cam.orthographicSize * cam.aspect * 2.5f, thickness, 1f);
                    break;
                case BorderSides.Left:
                    transform.position = new Vector2((-cam.orthographicSize * cam.aspect) - (thickness / 2f), 0f);
                    transform.localScale = new Vector3(thickness, cam.orthographicSize * 2.5f, 1f);
                    break;
                case BorderSides.Right:
                    transform.position = new Vector2((cam.orthographicSize * cam.aspect) + (thickness / 2f), 0f);
                    transform.localScale = new Vector3(thickness, cam.orthographicSize * 2.5f, 1f);
                    break;
            }
        }

        public enum BorderSides
        {
            Left,
            Right,
            Top,
            Bottom
        }
    }
}