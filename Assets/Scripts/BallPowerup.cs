using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class BallPowerup : MonoBehaviour
    {
        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}