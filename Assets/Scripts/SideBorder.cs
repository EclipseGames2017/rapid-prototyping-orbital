using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class SideBorder : MonoBehaviour
    {
        public BorderSides side;

        void Awake()
        {
            Camera cam = Camera.main;

            transform.position = new Vector3(cam.orthographicSize * cam.aspect * (float)side, 0f, 0f);

            transform.localScale = new Vector3(transform.localScale.x,
                                                cam.orthographicSize * 4f,
                                                1f);
        }

        public enum BorderSides
        {
            Left = -1,
            Right = 1
        }
    }
}