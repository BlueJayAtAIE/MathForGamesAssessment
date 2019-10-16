using System;
using System.Collections.Generic;

namespace MathFunctions
{
    class AABB
    {
        Vector3 min = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        Vector3 max = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        public AABB()
        {
            // Purposefully Blank.
        }

        public AABB(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Create an AABB from a list of points.
        /// </summary>
        /// <param name="points">List from which to make the AABB.</param>
        public void Fit(List<Vector3> points)
        {
            // Invalidate Min and Max so new values can replace them.
            min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }

        /// <summary>
        /// Create an AABB from an array of points.
        /// </summary>
        /// <param name="points">Array from which to make the AABB.</param>
        public void Fit(Vector3[] points)
        {
            // Invalidate Min and Max so new values can replace them.
            min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }
        }

        // Collision Checks --------------------

        // It's faster to check if they DONT collide since it'll excit the check sooner.
        public bool Overlaps(Vector3 p)
        {
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }

        public bool Overlaps(AABB otherCollider)
        {
            return !(max.x < otherCollider.min.x || max.y < otherCollider.min.y ||min.x > otherCollider.max.x || min.y > otherCollider.max.y);
        }

        public Vector3 ClosestPoint(Vector3 p)
        {
            return Vector3.Clamp(p, min, max);
        }

        // Finding other information from the AABB
        public Vector3 Center()
        {
            return (min + max) * 0.5f;
        }

        public Vector3 Extents()
        {
            return new Vector3(Math.Abs(max.x = min.x) * 0.5f, Math.Abs(max.y = min.y) * 0.5f, Math.Abs(max.z = min.z) * 0.5f);
        }

        public List<Vector3> Corners()
        {
            List<Vector3> corners = new List<Vector3>(4);
            corners[0] = min;
            corners[1] = new Vector3(min.x, max.y, min.z);
            corners[2] = max;
            corners[3] = new Vector3(max.x, min.y, max.z);
            return corners;
        }
    }
}
