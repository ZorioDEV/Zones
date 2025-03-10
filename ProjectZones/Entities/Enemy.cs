using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Collision;
using ProjectZones.Utilities;

namespace ProjectZones.Entities
{
    public class Enemy : Entity
    {
        public Rectangle Collider { get; private set; }
        private Color _color = Color.Red; // Default color
        public bool IsAlive { get; private set; } = true; // Track if the enemy is alive

        public Enemy(Rectangle bounds) : base(bounds)
        {
            // Create a collider twice the size of the Enemy's bounds, centered on the Enemy
            Collider = new Rectangle(
                bounds.X - bounds.Width,
                bounds.Y - bounds.Height,
                bounds.Width * 3,
                bounds.Height * 3
            );
        }

        public override void Update(GameTime gameTime, Quadrilateral collider, Triangle triangle1, Triangle triangle2)
        {
            // Enemy-specific behavior (e.g., chase the player, patrol an area)
            // For now, it just stands still
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsAlive) // Only draw if the enemy is alive
            {
                DrawingHelper.DrawRectangle(spriteBatch, Bounds, _color);
                DrawingHelper.DrawRectangleOutline(spriteBatch, Bounds, Color.DarkRed);
                DrawingHelper.DrawRectangleOutline(spriteBatch, Collider, Color.Green); // Draw collider outline
            }
        }

        public void CheckCollisionWithPlayer(Player player)
        {
            if (Collider.Intersects(player.Bounds))
            {
                _color = Color.Green; // Change color to green on collision
            }
            else
            {
                _color = Color.Red; // Revert to red if no collision
            }
        }

        public void CheckForRemoval(Player player, MouseState mouseState)
        {
            if (IsAlive && Collider.Intersects(player.Bounds)) // Check if player is in collider
            {
                if (mouseState.LeftButton == ButtonState.Pressed) // Check for left-click
                {
                    IsAlive = false; // Mark the enemy for removal
                }
            }
        }

        // Static method to spawn Enemies
        public static List<Enemy> RandomSpawnEnemies(int count, Random random, Viewport viewport)
        {
            List<Enemy> enemies = new List<Enemy>();
            for (int i = 0; i < count; i++)
            {
                int x = random.Next(0, viewport.Width - 25); // Random X within viewport
                int y = random.Next(0, viewport.Height - 25); // Random Y within viewport
                enemies.Add(new Enemy(new Rectangle(x, y, 25, 25)));
            }
            return enemies;
        }
    }
}
