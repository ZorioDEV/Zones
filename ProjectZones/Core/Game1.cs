using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Collision;
using ProjectZones.Entities;
using ProjectZones.Utilities;
using System;

namespace ProjectZones.Core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player _player;
        private NPC _npc;
        private Enemy _enemy;
        private Quadrilateral _quadrilateralCollider;
        private Triangle _triangle1;
        private Triangle _triangle2;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _player = new Player(new Rectangle(100, 100, 25, 25));
            _npc = new NPC(new Rectangle(400, 100, 25, 25));
            _enemy = new Enemy(new Rectangle(700, 100, 25, 25));

            // Define the quadrilateral collider
            _quadrilateralCollider = new Quadrilateral(
                new Vector2(200, 100), // Top-left
                new Vector2(300, 200), // Top-right
                new Vector2(300, 300), // Bottom-right
                new Vector2(200, 200)  // Bottom-left
            );

            // Define the triangles
            _triangle1 = new Triangle(new Vector2(200, 300), new Vector2(300, 300), new Vector2(200, 200));
            _triangle2 = new Triangle(new Vector2(300, 100), new Vector2(300, 200), new Vector2(200, 100));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DrawingHelper.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update(gameTime);

            // Update entities
            _player.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
            _npc.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
            _enemy.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw entities
            _player.Draw(_spriteBatch);
            _npc.Draw(_spriteBatch);
            _enemy.Draw(_spriteBatch);

            // Draw the triangles
            DrawingHelper.DrawFilledTriangle(_spriteBatch, _triangle1, Color.White);
            DrawingHelper.DrawFilledTriangle(_spriteBatch, _triangle2, Color.White);

            // Draw the quadrilateral collider outline
            DrawingHelper.DrawQuadrilateralOutline(_spriteBatch, _quadrilateralCollider, Color.Green);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
