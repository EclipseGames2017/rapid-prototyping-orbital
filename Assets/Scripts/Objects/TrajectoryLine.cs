using FallingSloth;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EclipseStudios.Orbital
{
    public class TrajectoryLine : MonoBehaviour
    {
        List<Vector2> points;

        public GameObject pointPrefab;
        GameObjectPool pointPool;

        [Range(2, 7)]
        public int dotCount = 5;

        public float minSpacing = 0.25f;
        public float maxSpacing = 1.0f;

        Launcher launcher;

        void Start()
        {
            launcher = GetComponent<Launcher>();
        }

        void OnEnable()
        {
            if (pointPool == null)
                pointPool = new GameObjectPool(pointPrefab, dotCount, false);

            points = new List<Vector2>();
            for (int i = 0; i < dotCount; i++)
                points.Add(Vector2.zero);
            
            for (int i = 0; i < dotCount; i++)
            {
                GameObject temp = pointPool.GetObject();
                temp.SetActive(true);
                temp.transform.parent = transform;
            }
        }

        public void UpdatePoints(float shotPower)
        {
            float spacing = shotPower.RemapRange(launcher.minMagnitude, launcher.maxMagnitude, minSpacing, maxSpacing);
            float x = 0.0f;
            float y = 1.0f;
            foreach (GameObject temp in pointPool.GetActiveObjects())
            {
                temp.transform.localPosition = new Vector2(x, y);
                y += spacing;
            }
        }

        void OnDisable()
        {
            System.Array.ForEach(pointPool.GetActiveObjects(), obj => obj.SetActive(false));
        }
    }
}