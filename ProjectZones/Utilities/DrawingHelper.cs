using ProjectZones.Collision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjectZones.Utilities
{
    public static class DrawingHelper
    {
        private static Texture2D _pixel;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(_pixel, rectangle, color);
        }

        public static void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(_pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), color);
            spriteBatch.Draw(_pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width, 1), color);
            spriteBatch.Draw(_pixel, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), color);
            spriteBatch.Draw(_pixel, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, 1, rectangle.Height), color);
        }

        public static void DrawQuadrilateralOutline(SpriteBatch spriteBatch, Quadrilateral quadrilateral, Color color)
        {
            DrawLine(spriteBatch, quadrilateral.Vertex1, quadrilateral.Vertex2, color);
            DrawLine(spriteBatch, quadrilateral.Vertex2, quadrilateral.Vertex3, color);
            DrawLine(spriteBatch, quadrilateral.Vertex3, quadrilateral.Vertex4, color);
            DrawLine(spriteBatch, quadrilateral.Vertex4, quadrilateral.Vertex1, color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);

            spriteBatch.Draw(_pixel,
                             start,
                             null,
                             color,
                             angle,
                             Vector2.Zero,
                             new Vector2(edge.Length(), 1),
                             SpriteEffects.None,
                             0);
        }

        public static void DrawFilledTriangle(SpriteBatch spriteBatch, Triangle triangle, Color color)
        {
            // Calculate the bounding box of the triangle
            int minX = (int)Math.Min(Math.Min(triangle.Vertex1.X, triangle.Vertex2.X), triangle.Vertex3.X);
            int minY = (int)Math.Min(Math.Min(triangle.Vertex1.Y, triangle.Vertex2.Y), triangle.Vertex3.Y);
            int maxX = (int)Math.Max(Math.Max(triangle.Vertex1.X, triangle.Vertex2.X), triangle.Vertex3.X);
            int maxY = (int)Math.Max(Math.Max(triangle.Vertex1.Y, triangle.Vertex2.Y), triangle.Vertex3.Y);

            // Iterate over each pixel in the bounding box
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Vector2 point = new Vector2(x, y);
                    if (GeometryHelper.IsPointInsideTriangle(point, triangle))
                    {
                        spriteBatch.Draw(_pixel, new Rectangle(x, y, 1, 1), color);
                    }
                }
            }
        }
    }
}
