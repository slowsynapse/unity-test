using System;
using UnityEngine;

namespace Utils.injection
{
    public static class Geometry
    {
        
        public static bool LineIntersect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            var isIntersecting = false;

            var denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

            //Make sure the denominator is > 0, if so the lines are parallel
            if (Math.Abs(denominator) > 0.00001f)
            {
                var uA = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
                var uB = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

                //Is intersecting if u_a and u_b are between 0 and 1
                if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
                {
                    isIntersecting = true;
                }
            }

            return isIntersecting;
        }

        public static Vector2 ProjectPointLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            var rhs = point - lineStart;
            var vector3 = lineEnd - lineStart;
            var magnitude = vector3.magnitude;
            var lhs = vector3;
            if (magnitude > 9.999999974752427E-07)
                lhs /= magnitude;
            var num = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0.0f, magnitude);
            return lineStart + lhs * num;
        }
    }
}