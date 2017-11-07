using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class Particle : MonoBehaviour
    {
        [HideInInspector]
        new public Rigidbody2D rigidbody2D;

        void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (collision2D.gameObject.tag == "LauncherPad")
            {
                Destroy();
            }
        }

        void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}