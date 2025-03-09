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
        public Enemy(Rectangle bounds) : base(bounds) { }

        public override void Update(GameTime gameTime, Quadrilateral collider, Triangle triangle1, Triangle triangle2)
        {
            // Enemy-specific behavior (e.g., chase the player, patrol an area)
            // For now, it just stands still
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawingHelper.DrawRectangle(spriteBatch, Bounds, Color.Red);
            DrawingHelper.DrawRectangleOutline(spriteBatch, Bounds, Color.DarkRed);
        }
    }
}
