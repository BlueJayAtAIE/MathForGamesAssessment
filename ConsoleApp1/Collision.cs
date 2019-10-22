using System;
using System.Collections.Generic;

namespace MathFunctions
{
    class AABB
    {
        public Vector3 min = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
        public Vector3 max = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        public AABB()
        {
            // Purposefully Blank.
        }

        public AABB(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public void Resize(Vector3 min, Vector3 max)
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

        // It's faster to check if they DONT collide since it'll exit the check sooner.
        public bool Overlaps(Vector3 p)
        {
            return !(p.x < min.x || p.y < min.y || p.x > max.x || p.y > max.y);
        }

        public bool Overlaps(AABB otherCollider)
        {
            return !(max.x < otherCollider.min.x || max.y < otherCollider.min.y || min.x > otherCollider.max.x || min.y > otherCollider.max.y);
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
            List<Vector3> temp = new List<Vector3>(4) { min, new Vector3(min.x, max.y, min.z), max, new Vector3(max.x, min.y, max.z) };
            return temp;
        }
    }

    class Sphere
    {
        public Vector3 center;
        public float radius;

        public Sphere()
        {
            // Purposefully Blank.
        }

        public Sphere(Vector3 p, float r)
        {
            center = p;
            radius = r;
        }

        public void Resize(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
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

    class Ray
    {
        Vector3 origin;
        Vector3 direction;
        float length;

        public Ray()
        {
            // Purposefully blank.
        }

        public Ray(Vector3 start, Vector3 direction, float length = float.MaxValue)
        {
            origin = start;
            this.direction = direction;
            this.length = length;
        }

        float Clamp(float t, float a, float b)
        {
            return Math.Max(a, Math.Min(b, t));
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            // Ray origin to arbirary point.
            Vector3 p = point - origin;

            // Project the point onto the ray and clamp by length.
            float t = Clamp(p.DotProduct(direction), 0, length);

            // Return position in direction of the ray.
            return origin + direction * t;
        }

        public bool Intersects(Sphere sphere, Vector3 I = null)
        {
            // Ray origin to Sphere center.
            Vector3 L = sphere.center - origin;

            // Project Sphere onto Ray.
            float t = L.DotProduct(direction);

            // Get Sqr Distance from Sphere center to Ray.
            float dd = L.DotProduct(L) - t * t;

            // Subtract penetration amount from projected distance.
            t -= (float)Math.Sqrt(sphere.radius * sphere.radius - dd);

            // It intersects if within Ray length.
            if (t >= 0 && t <= length)
            {
                // Store intersection point if requested.
                if (I != null)
                {
                    I = origin + direction * t;
                }
                return true;
            }

            // Defalt; no intersection.
            return false;
        }

        public bool Interects(AABB aabb, Vector3 I = null)
        {
            float xmin, xmax, ymin, ymax;

            if (direction.x < 0)
            {
                // If ray's direction is negitive, this will run.
                xmin = (aabb.max.x - origin.x) / direction.x;
                xmax = (aabb.min.x - origin.x) / direction.x;
            }
            else
            {
                xmin = (aabb.min.x - origin.x) / direction.x;
                xmax = (aabb.max.x - origin.x) / direction.x;
            }

            if (direction.y < 0)
            {
                // If ray's direction is negitive, this will run.
                ymin = (aabb.max.y - origin.y) / direction.y;
                ymax = (aabb.min.y - origin.y) / direction.y;
            }
            else
            {
                ymin = (aabb.min.y - origin.y) / direction.y;
                ymax = (aabb.max.y - origin.y) / direction.y;
            }

            // Completely encompased by box.
            if (xmin > ymax || ymin > xmax) return false;

            // First conteact is larger of two min.
            float t = Math.Max(xmin, ymin);

            // Intersects if within range.
            if (t >= 0 && t <= length)
            {
                // Store intersection point is needed.
                if (I != null)
                {
                    I = origin + direction * t;
                }
                return true;
            }

            // Not within range.
            return false;
        }
    }
}
