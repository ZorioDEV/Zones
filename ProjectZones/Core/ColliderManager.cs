using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Entities;
using ProjectZones.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectZones.Core
{
    public class ColliderManager
    {
        private List<Rectangle> _colliders;
        private Random _random;
        private Viewport _viewport;

        public ColliderManager(Viewport viewport, int numberOfColliders)
        {
            _viewport = viewport;
            _random = new Random();
            _colliders = new List<Rectangle>();

            SpawnColliders(numberOfColliders);
        }

        private void SpawnColliders(int numberOfColliders)
        {
            for (int i = 0; i < numberOfColliders; i++)
            {
                int width = 100;
                int height = 100;
                int x = _random.Next(0, _viewport.Width - width);
                int y = _random.Next(0, _viewport.Height - height);

                _colliders.Add(new Rectangle(x, y, width, height));
            }
        }

        public void Update(Player player)
        {
            foreach (var collider in _colliders)
            {
                if (player.Bounds.Intersects(collider))
                {
                    // Handle collision (e.g., prevent player from moving through the collider)
                    Rectangle intersection = Rectangle.Intersect(player.Bounds, collider);
                    if (intersection.Width < intersection.Height)
                    {
                        if (player.Bounds.X < collider.X)
                            player.Bounds = new Rectangle(collider.X - player.Bounds.Width, player.Bounds.Y, player.Bounds.Width, player.Bounds.Height);
                        else
                            player.Bounds = new Rectangle(collider.Right, player.Bounds.Y, player.Bounds.Width, player.Bounds.Height);
                    }
                    else
                    {
                        if (player.Bounds.Y < collider.Y)
                            player.Bounds = new Rectangle(player.Bounds.X, collider.Y - player.Bounds.Height, player.Bounds.Width, player.Bounds.Height);
                        else
                            player.Bounds = new Rectangle(player.Bounds.X, collider.Bottom, player.Bounds.Width, player.Bounds.Height);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var collider in _colliders)
            {
                DrawingHelper.DrawRectangle(spriteBatch, collider, Color.Blue);
                DrawingHelper.DrawRectangleOutline(spriteBatch, collider, Color.Cyan);
            }
        }
    }
}
