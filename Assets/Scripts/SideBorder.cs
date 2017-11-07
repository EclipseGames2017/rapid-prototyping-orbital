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

        void Awake()
        {
            // Get the main camera.
            Camera cam = Camera.main;

            // Used to determine if the position should be positive or negative.
            float sign = (side == BorderSides.Left || side == BorderSides.Bottom) ? -1f : 1f;

            // Used to offset the borders outwards.
            float offset = .4f;

            switch (side)
            {
                case BorderSides.Top:
                case BorderSides.Bottom:
                    transform.position = new Vector3(0f, (cam.orthographicSize + offset) * sign, 0f);
                    transform.localScale = new Vector3(cam.orthographicSize * cam.aspect * 4f,
                                                        transform.localScale.y,
                                                        1f);
                    break;
                case BorderSides.Left:
                case BorderSides.Right:
                    transform.position = new Vector3(((cam.orthographicSize * cam.aspect) + offset) * sign, 0f, 0f);
                    transform.localScale = new Vector3(transform.localScale.x,
                                                        cam.orthographicSize * 4f,
                                                        1f);
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