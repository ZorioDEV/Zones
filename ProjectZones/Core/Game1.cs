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
        private List<NPC> _npcs;
        private List<Enemy> _enemies;
        private Quadrilateral _quadrilateralCollider;
        private Triangle _triangle1;
        private Triangle _triangle2;

        private ColliderManager _colliderManager;

        private KeyboardState _previousKeyboardState;
        private KeyboardState currentKeyboardState;
        private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        private Random _random = new Random();

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

            // Initialize lists
            _npcs = NPC.RandomSpawnNPCs(5, _random, GraphicsDevice.Viewport); // Spawn 5 NPCs
            _enemies = Enemy.RandomSpawnEnemies(5, _random, GraphicsDevice.Viewport); // Spawn 5 Enemies

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

            // Initialize the collider manager
            _colliderManager = new ColliderManager(GraphicsDevice.Viewport, 10); // Spawn 10 collide

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
            // Update the previous keyboard and mouse states at the beginning of the Update method
            _previousKeyboardState = currentKeyboardState;
            _previousMouseState = _currentMouseState;

            // Get the current keyboard and mouse states
            currentKeyboardState = Keyboard.GetState();
            _currentMouseState = Mouse.GetState();


            if (currentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            // Toggle fullscreen on F11 key press (only once per press)
            if (currentKeyboardState.IsKeyDown(Keys.F11) && _previousKeyboardState.IsKeyUp(Keys.F11))
            {
                _graphics.ToggleFullScreen();
                _graphics.ApplyChanges();
                _viewportHelper.Update(_graphics.GraphicsDevice, _graphics.IsFullScreen); // Update viewport helper
            }

            InputManager.Update(gameTime);

            // Update entities
            _player.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);

            // Update collider manager
            _colliderManager.Update(_player);

            // Update NPCs
            foreach (var npc in _npcs)
            {
                npc.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
                npc.CheckCollisionWithPlayer(_player, currentKeyboardState, _previousKeyboardState);
            }

            // Update Enemies
            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                enemy.Update(gameTime, _quadrilateralCollider, _triangle1, _triangle2);
                enemy.CheckCollisionWithPlayer(_player);
                enemy.CheckForRemoval(_player, _currentMouseState);

                // Remove enemy if it's no longer alive
                if (!enemy.IsAlive)
                {
                    _enemies.RemoveAt(i);
                }
            }

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

            // Draw NPCs
            foreach (var npc in _npcs)
            {
                npc.Draw(_spriteBatch);
            }

            // Draw Enemies
            foreach (var enemy in _enemies)
            {
                enemy.Draw(_spriteBatch);
            }

            // Draw the triangles
            DrawingHelper.DrawFilledTriangle(_spriteBatch, _triangle1, Color.White);
            DrawingHelper.DrawFilledTriangle(_spriteBatch, _triangle2, Color.White);

            // Draw the quadrilateral collider outline
            DrawingHelper.DrawQuadrilateralOutline(_spriteBatch, _quadrilateralCollider, Color.Green);

            // Draw colliders
            _colliderManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
