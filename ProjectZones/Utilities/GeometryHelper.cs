using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Collision;
using ProjectZones.Core;

namespace ProjectZones.Utilities
{
    public static class GeometryHelper
    {
        public static bool IsPointInsideTriangle(Vector2 point, Triangle triangle)
        {
            float area = 0.5f * (-triangle.Vertex2.Y * triangle.Vertex3.X + triangle.Vertex1.Y * (-triangle.Vertex2.X + triangle.Vertex3.X) +
                                 triangle.Vertex1.X * (triangle.Vertex2.Y - triangle.Vertex3.Y) + triangle.Vertex2.X * triangle.Vertex3.Y);

            float s = 1 / (2 * area) * (triangle.Vertex1.Y * triangle.Vertex3.X - triangle.Vertex1.X * triangle.Vertex3.Y +
                                        (triangle.Vertex3.Y - triangle.Vertex1.Y) * point.X + (triangle.Vertex1.X - triangle.Vertex3.X) * point.Y);
            float t = 1 / (2 * area) * (triangle.Vertex1.X * triangle.Vertex2.Y - triangle.Vertex1.Y * triangle.Vertex2.X +
                                        (triangle.Vertex1.Y - triangle.Vertex2.Y) * point.X + (triangle.Vertex2.X - triangle.Vertex1.X) * point.Y);

            return s >= 0 && t >= 0 && (s + t) <= 1;
        }

        public static bool DoEdgesIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            float denominator = ((a2.X - a1.X) * (b2.Y - b1.Y)) - ((a2.Y - a1.Y) * (b2.X - b1.X));

            if (denominator == 0) // Lines are parallel
                return false;

            float numerator1 = ((a1.Y - b1.Y) * (b2.X - b1.X)) - ((a1.X - b1.X) * (b2.Y - b1.Y));
            float numerator2 = ((a1.Y - b1.Y) * (a2.X - a1.X)) - ((a1.X - b1.X) * (a2.Y - a1.Y));

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}