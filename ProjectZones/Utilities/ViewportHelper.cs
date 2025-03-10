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
    public class ViewportHelper
    {
        // Rendering resolution
        public int RenderWidth { get; private set; }
        public int RenderHeight { get; private set; }

        // Viewport and scaling
        public Viewport Viewport { get; private set; }
        public Matrix ScaleMatrix { get; private set; }

        // Target aspect ratio (e.g., 16:9)
        private const float TargetAspectRatio = 16f / 9f;

        private GraphicsDeviceManager _graphics;

        public ViewportHelper(GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            CalculateRenderingResolution(_graphics.GraphicsDevice);
            UpdateViewport(_graphics.GraphicsDevice, _graphics.IsFullScreen);
        }

        public void Update(GraphicsDevice graphicsDevice, bool isFullScreen)
        {
            CalculateRenderingResolution(graphicsDevice);
            UpdateViewport(graphicsDevice, isFullScreen);
        }

        private void CalculateRenderingResolution(GraphicsDevice graphicsDevice)
        {
            // Calculate the rendering resolution based on the target aspect ratio
            RenderWidth = graphicsDevice.Viewport.Width;
            RenderHeight = (int)(RenderWidth / TargetAspectRatio);

            // If the calculated height exceeds the screen height, adjust the width instead
            if (RenderHeight > graphicsDevice.Viewport.Height)
            {
                RenderHeight = graphicsDevice.Viewport.Height;
                RenderWidth = (int)(RenderHeight * TargetAspectRatio);
            }
        }

        private void UpdateViewport(GraphicsDevice graphicsDevice, bool isFullScreen)
        {
            if (isFullScreen)
            {
                // Fullscreen mode: stretch the viewport to fill the entire screen
                Viewport = new Viewport(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

                // Calculate the scaling factor to stretch the rendering to fit the screen
                float scaleX = (float)graphicsDevice.Viewport.Width / RenderWidth;
                float scaleY = (float)graphicsDevice.Viewport.Height / RenderHeight;
                ScaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            }
            else
            {
                // Windowed mode: maintain the target aspect ratio with black bars
                float scaleX = (float)graphicsDevice.Viewport.Width / RenderWidth;
                float scaleY = (float)graphicsDevice.Viewport.Height / RenderHeight;
                float scale = Math.Min(scaleX, scaleY);

                // Calculate the new viewport dimensions
                int viewportWidth = (int)(RenderWidth * scale);
                int viewportHeight = (int)(RenderHeight * scale);

                // Center the viewport
                int viewportX = (graphicsDevice.Viewport.Width - viewportWidth) / 2;
                int viewportY = (graphicsDevice.Viewport.Height - viewportHeight) / 2;

                // Set the viewport
                Viewport = new Viewport(viewportX, viewportY, viewportWidth, viewportHeight);

                // Create a scaling matrix
                ScaleMatrix = Matrix.CreateScale(scale, scale, 1.0f);
            }
        }
    }
}