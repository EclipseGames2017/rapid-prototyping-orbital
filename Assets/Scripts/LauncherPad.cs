using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class LauncherPad : MonoBehaviour
    {
        void Awake()
        {
            Camera cam = Camera.main;

            transform.position = new Vector3(0f, -cam.orthographicSize, 0f);

            transform.localScale = new Vector3(cam.orthographicSize * cam.aspect * 4f,
                                                transform.localScale.y,
                                                1f);
        }
    }
}