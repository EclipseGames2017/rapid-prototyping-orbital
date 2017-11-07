using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class ParticleDeathEffect : MonoBehaviour
    {
        void OnEnable()
        {
            Invoke("Destroy", 2f);
        }

        void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}