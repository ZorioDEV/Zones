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
    public class NPC : Entity
    {
        public Rectangle Collider { get; private set; }
        private Color _color = Color.Blue; // Default color
        public bool HasBeenTalkedTo { get; private set; } = false; // Track if the player has talked to the NPC

        public NPC(Rectangle bounds) : base(bounds)
        {
            // Create a collider twice the size of the NPC's bounds, centered on the NPC
            Collider = new Rectangle(
                bounds.X - bounds.Width,
                bounds.Y - bounds.Height,
                bounds.Width * 3,
                bounds.Height * 3
            );
        }

        public override void Update(GameTime gameTime, Quadrilateral collider, Triangle triangle1, Triangle triangle2)
        {
            // NPC-specific behavior (e.g., follow a path, avoid obstacles)
            // For now, it just stands still
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Change color to gray if the NPC has been talked to
            Color drawColor = HasBeenTalkedTo ? Color.Gray : _color;
            DrawingHelper.DrawRectangle(spriteBatch, Bounds, drawColor);
            DrawingHelper.DrawRectangleOutline(spriteBatch, Bounds, Color.DarkBlue);
            DrawingHelper.DrawRectangleOutline(spriteBatch, Collider, Color.Green); // Draw collider outline
        }

        public void CheckCollisionWithPlayer(Player player, KeyboardState currentKeyboardState, KeyboardState previousKeyboardState)
        {
            if (Collider.Intersects(player.Bounds))
            {
                // Change color to green when the player is inside the collider
                if (!HasBeenTalkedTo)
                {
                    _color = Color.Green;
                }

                // Check for single key press of the E key
                if (currentKeyboardState.IsKeyDown(Keys.E) && previousKeyboardState.IsKeyUp(Keys.E))
                {
                    HasBeenTalkedTo = true; // Mark the NPC as talked to
                    _color = Color.Gray; // Permanently change color to gray
                }
            }
            else if (!HasBeenTalkedTo)
            {
                // Revert to blue only if the NPC has not been talked to
                _color = Color.Blue;
            }
        }

        // Static method to spawn NPCs
        public static List<NPC> RandomSpawnNPCs(int count, Random random, Viewport viewport)
        {
            List<NPC> npcs = new List<NPC>();
            for (int i = 0; i < count; i++)
            {
                int x = random.Next(0, viewport.Width - 25); // Random X within viewport
                int y = random.Next(0, viewport.Height - 25); // Random Y within viewport
                npcs.Add(new NPC(new Rectangle(x, y, 25, 25)));
            }
            return npcs;
        }
    }
}
