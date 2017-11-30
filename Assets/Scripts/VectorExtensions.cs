using UnityEngine;

namespace EclipseStudios.Orbital
{
    public static class VectorExtensions
    {
        public static Vector3 Round(this Vector3 v)
        {
            float x = Mathf.Round(v.x);
            float y = Mathf.Round(v.y);
            float z = Mathf.Round(v.z);
            return new Vector3(x, y, z);
        }
    }
}
