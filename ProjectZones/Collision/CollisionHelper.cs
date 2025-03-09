using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Core;
using ProjectZones.Utilities;

namespace ProjectZones.Collision
{
    public static class CollisionHelper
    {
        public static bool IsRectangleInsideQuadrilateral(Rectangle rectangle, Quadrilateral quadrilateral)
        {
            Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);
            Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);

            return IsPointInsideQuadrilateral(topLeft, quadrilateral) &&
                   IsPointInsideQuadrilateral(topRight, quadrilateral) &&
                   IsPointInsideQuadrilateral(bottomRight, quadrilateral) &&
                   IsPointInsideQuadrilateral(bottomLeft, quadrilateral);
        }

        public static bool IsPointInsideQuadrilateral(Vector2 point, Quadrilateral quadrilateral)
        {
            Triangle triangle1 = new Triangle(quadrilateral.Vertex1, quadrilateral.Vertex2, quadrilateral.Vertex3);
            Triangle triangle2 = new Triangle(quadrilateral.Vertex1, quadrilateral.Vertex3, quadrilateral.Vertex4);

            return GeometryHelper.IsPointInsideTriangle(point, triangle1) || GeometryHelper.IsPointInsideTriangle(point, triangle2);
        }

        public static bool DoRectangleAndTriangleIntersect(Rectangle rectangle, Triangle triangle)
        {
            // Check if any rectangle vertex is inside the triangle
            Vector2[] rectVertices = new Vector2[]
            {
                new Vector2(rectangle.Left, rectangle.Top),
                new Vector2(rectangle.Right, rectangle.Top),
                new Vector2(rectangle.Right, rectangle.Bottom),
                new Vector2(rectangle.Left, rectangle.Bottom)
            };

            foreach (var vertex in rectVertices)
            {
                if (GeometryHelper.IsPointInsideTriangle(vertex, triangle))
                    return true;
            }

            // Check if any triangle vertex is inside the rectangle
            foreach (var vertex in triangle.GetVertices())
            {
                if (rectangle.Contains(vertex))
                    return true;
            }

            // Check if any edge of the rectangle intersects with any edge of the triangle
            for (int i = 0; i < rectVertices.Length; i++)
            {
                Vector2 rectStart = rectVertices[i];
                Vector2 rectEnd = rectVertices[(i + 1) % rectVertices.Length];

                for (int j = 0; j < triangle.GetVertices().Length; j++)
                {
                    Vector2 triStart = triangle.GetVertices()[j];
                    Vector2 triEnd = triangle.GetVertices()[(j + 1) % triangle.GetVertices().Length];

                    if (GeometryHelper.DoEdgesIntersect(rectStart, rectEnd, triStart, triEnd))
                        return true;
                }
            }

            return false;
        }
    }
}
