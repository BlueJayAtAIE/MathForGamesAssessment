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

    class Sphere
    {
        Vector3 center;
        float radius;

        public Sphere()
        {
            // Purposefully Blank.
        }

        public Sphere(Vector3 p, float r)
        {
            center = p;
            radius = r;
        }

        public void Fit(Vector3[] points)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int i = 0; i < points.Length; i++)
            {
                min = Vector3.Min(min, points[i]);
                max = Vector3.Max(max, points[i]);
            }

            center = (min + max) * 0.5f;
            radius = center.Distance(max);
        }

        public void Fit(List<Vector3> points)
        {
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (Vector3 p in points)
            {
                min = Vector3.Min(min, p);
                max = Vector3.Max(max, p);
            }

            center = (min + max) * 0.5f;
            radius = center.Distance(max);
        }

        // TODO: Another method of fitting a Sphere to a collection of points is to first find the average position within
        // the collection and set it to the Sphere’s center, then set the radius to the distance between the
        // center and the point farthest from the center.This method requires looping through the points multiple times.
        // Try and implement this second method yourself.

        // Collision Checks --------------------
        public bool Overlaps(Vector3 p)
        {
            Vector3 toPoint = p - center;
            return toPoint.MagnitudeSqr() <= (radius * radius);
        }

        public bool Overlaps(Sphere otherCollider)
        {
            Vector3 diff = otherCollider.center - center;
            float r = radius + otherCollider.radius;
            return diff.MagnitudeSqr() <= (r * r);
        }

        public bool Overlaps(AABB aabb)
        {
            Vector3 diff = aabb.ClosestPoint(center) - center;
            return diff.DotProduct(diff) <= (radius * radius);
        }

        Vector3 ClosestPoint(Vector3 p)
        {
            // Distance from the center.
            Vector3 toPoint = p - center;

            // If outside the radius, bring it back to the radius.
            if (toPoint.MagnitudeSqr() > (radius * radius))
            {
                toPoint = toPoint.GetNormalized() * radius;
            }
            return center + toPoint;
        }
    }
}
