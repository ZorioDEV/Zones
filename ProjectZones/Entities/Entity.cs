using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Collision;

namespace ProjectZones.Entities
{
    public abstract class Entity
    {
        // Common properties for all entities
        public Rectangle Bounds { get; set; }
        protected float MoveSpeed { get; set; } = 150f;
        protected float ReducedMoveSpeed { get; set; } = 100f;

        // Constructor
        public Entity(Rectangle bounds)
        {
            Bounds = bounds;
        }

        // Abstract methods for updating and drawing
        public abstract void Update(GameTime gameTime, Quadrilateral collider, Triangle triangle1, Triangle triangle2);
        public abstract void Draw(SpriteBatch spriteBatch);

        // Common collision detection method
        protected bool CheckCollisionWithTriangles(Triangle triangle1, Triangle triangle2)
        {
            return CollisionHelper.DoRectangleAndTriangleIntersect(Bounds, triangle1) ||
                   CollisionHelper.DoRectangleAndTriangleIntersect(Bounds, triangle2);
        }

        // Normalize the movement vector
        protected Vector2 NormalizeMovement(Vector2 movement)
        {
            if (movement != Vector2.Zero)
            {
                movement.Normalize(); // Normalize the vector to have a magnitude of 1
            }
            return movement;
        }

        // Common movement method
        protected void Move(Vector2 movement, float speed, GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Bounds = new Rectangle(
                Bounds.X + (int)(movement.X * speed * deltaTime),
                Bounds.Y + (int)(movement.Y * speed * deltaTime),
                Bounds.Width,
                Bounds.Height
            );
        }
    }
}
