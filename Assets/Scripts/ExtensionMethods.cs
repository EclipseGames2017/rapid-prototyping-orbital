using UnityEngine;

namespace EclipseStudios.Orbital
{
    public static class ExtensionMethods
    {
        public static Vector3 Round(this Vector3 v)
        {
            float x = Mathf.Round(v.x);
            float y = Mathf.Round(v.y);
            float z = Mathf.Round(v.z);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Remaps a float t from the range a-b into the range c-d
        /// </summary>
        public static float RemapRange(this float t, float a, float b, float c, float d)
        {
            return c + ((d - c) / (b - a)) * (t - a);
        }
    }
}
