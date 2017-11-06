using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FallingSloth
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T Instance { get; private set; }

        public bool persistent = true;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            else
            {
                Instance = this as T;

                if (persistent)
                    DontDestroyOnLoad(gameObject);
            }
        }
    }
}