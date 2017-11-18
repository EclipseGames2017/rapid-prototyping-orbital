using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class DeathEffect : MonoBehaviour
    {
        public float lifetime = 2f;

        void OnEnable()
        {
            Invoke("Destroy", lifetime);
        }

        void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}