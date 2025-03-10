using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectZones.Collision;
using ProjectZones.Entities;
using ProjectZones.Utilities;
using ProjectZones.World;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectZones.Core
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ViewportHelper _viewportHelper;

        private Player _player;
        private NPC _npc;
        private Enemy _enemy;
        private Quadrilateral _quadrilateralCollider;
        private Triangle _triangle1;
        private Triangle _triangle2;

        private KeyboardState _previousKeyboardState;
        private KeyboardState currentKeyboardState;

        // TUTORIAL 
        private Texture2D tilemapAsset;
        private Sprite sprite;
        private ScaledSprite scaledSprite;
        private ColoredSprite coloredSprite;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Ensure the window is not borderless
            Window.IsBorderless = false;

            // Set initial windowed mode
            _graphics.PreferredBackBufferWidth = 1280; // Example: 1280x720
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
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

            // Initialize the viewport helper
            _viewportHelper = new ViewportHelper(_graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DrawingHelper.Initialize(GraphicsDevice);

            // TUTORIAL 
            tilemapAsset = Content.Load<Texture2D>("tilemap_packed");
            sprite = new Sprite(tilemapAsset, Vector2.Zero);
            scaledSprite = new ScaledSprite(tilemapAsset, Vector2.Zero);
            coloredSprite = new ColoredSprite(tilemapAsset, Vector2.Zero, Color.Red);
        }

        protected override void Update(GameTime gameTime)
        {
            // Get the current keyboard state
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Toggle fullscreen on F11 key press (only once per press)
            if (currentKeyboardState.IsKeyDown(Keys.F11) && _previousKeyboardState.IsKeyUp(Keys.F11))
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
                _viewportHelper.Update(_graphics.GraphicsDevice, _graphics.IsFullScreen); // Update viewport helper
            }

            // Update the previous keyboard state
            _previousKeyboardState = currentKeyboardState;

            InputManager.Update(gameTime);

            // Update entities
            _player.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
            _npc.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
            _enemy.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Set the viewport and scaling
            GraphicsDevice.Viewport = _viewportHelper.Viewport;
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _viewportHelper.ScaleMatrix);

            // TUTORIAL 
            _spriteBatch.Draw(
                tilemapAsset,
                //new Vector2(500, 500),
                new Rectangle(500, 500, 864, 576),
                Color.White
            );
            //_spriteBatch.Draw(
            //    sprite.texture,
            //    sprite.position,
            //    Color.White
            //);
            //_spriteBatch.Draw(
            //    scaledSprite.texture,
            //    scaledSprite.Rect,
            //    Color.White
            //);
            _spriteBatch.Draw(
                coloredSprite.texture,
                coloredSprite.Rect,
                coloredSprite.color
            );

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
