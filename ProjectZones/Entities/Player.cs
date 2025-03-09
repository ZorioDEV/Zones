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
using ProjectZones.Utilities;

namespace ProjectZones.Entities
{
    public class Player : Entity
    {
        public Player(Rectangle bounds) : base(bounds) { }

        public override void Update(GameTime gameTime, Quadrilateral collider, Triangle triangle1, Triangle triangle2)
        {
            Rectangle previousBounds = Bounds;

            // Get movement input
            Vector2 movement = Vector2.Zero;

            if (InputManager.IsKeyDown(Keys.W))
                movement.Y -= 1;
            if (InputManager.IsKeyDown(Keys.S))
                movement.Y += 1;
            if (InputManager.IsKeyDown(Keys.A))
                movement.X -= 1;
            if (InputManager.IsKeyDown(Keys.D))
                movement.X += 1;

            // Normalize the movement vector to ensure consistent speed
            movement = NormalizeMovement(movement);

            // Apply movement speed
            float currentSpeed = MoveSpeed;
            if (CollisionHelper.IsRectangleInsideQuadrilateral(Bounds, collider))
            {
                // Adjust player position based on movement direction
                bool isMovingLeft = InputManager.IsKeyDown(Keys.A);
                bool isMovingRight = InputManager.IsKeyDown(Keys.D);
                bool isMovingUp = InputManager.IsKeyDown(Keys.W);
                bool isMovingDown = InputManager.IsKeyDown(Keys.S);

                // Only apply vector shift if moving left or right, and not moving up or down
                if (isMovingLeft && !isMovingUp && !isMovingDown)
                {
                    Bounds = new Rectangle(Bounds.X, Bounds.Y - 1, Bounds.Width, Bounds.Height); // Shift upward
                }
                else if (isMovingRight && !isMovingUp && !isMovingDown)
                {
                    Bounds = new Rectangle(Bounds.X, Bounds.Y + 1, Bounds.Width, Bounds.Height); // Shift downward
                }

                // Reduce movement speed
                currentSpeed = ReducedMoveSpeed;
            }

            // Apply movement using the base class method
            Move(movement, currentSpeed, gameTime);

            // Check for collisions with triangles
            if (CheckCollisionWithTriangles(triangle1, triangle2))
            {
                // Revert to the previous position if there's a collision
                Bounds = previousBounds;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawingHelper.DrawRectangle(spriteBatch, Bounds, Color.White);
            DrawingHelper.DrawRectangleOutline(spriteBatch, Bounds, Color.Red);
        }
    }
}
