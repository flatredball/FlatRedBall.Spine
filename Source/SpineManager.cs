using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRedBall.Spine
{
    public static class SpineManager
    {
        public static SkeletonRenderer SkeletonRenderer { get; private set; }

        public static bool IsInitialized => SkeletonRenderer != null;

        public static void Initialize(GraphicsDevice graphicsDevice, Effect spineEffect)
        {
            SkeletonRenderer = new SkeletonRenderer(graphicsDevice);
            SkeletonRenderer.PremultipliedAlpha = false;
            SkeletonRenderer.Effect = spineEffect;

        }

        static Matrix lastWorldMatrix = Matrix.Identity;
        static float lastViewportWidth = 0;
        static float lastViewportHeight = 0;

        public static void PrepareDraw(Camera camera)
        {
            if(SkeletonRenderer == null)
            {
                throw new InvalidOperationException("SkeletonRenderer is null - did you remember to call SpineManager.Initialize?");
            }

            var zoom = camera.DestinationRectangle.Height / camera.OrthogonalHeight;

            var world =
                Matrix.CreateTranslation(-camera.X, camera.Y, 0) *
                Matrix.CreateScale(zoom, zoom, 1) 
                ;
            var graphicsDevice = FlatRedBallServices.GraphicsDevice;

            var viewportWidth = graphicsDevice.Viewport.Width;
            var viewportHeight = graphicsDevice.Viewport.Height;

            // If it's the same, no sense in doing this again...
            if (world != lastWorldMatrix || viewportWidth != lastViewportHeight || viewportHeight != lastViewportHeight)
            {
                lastWorldMatrix = world;
                lastViewportWidth = viewportWidth;
                lastViewportHeight = viewportHeight;

                if(SkeletonRenderer.Effect == null)
                {
                    throw new InvalidOperationException("The SkeletonRenderer Effect is null. You must first load the Spine effect and assign it to SkeletonRenderer.Effect before any Spine objects are drawn.");
                }
                var effect = SkeletonRenderer.Effect;
                effect.Parameters["World"].SetValue(world);
                effect.Parameters["View"].SetValue(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up));


                var projectionMatrix = Matrix.CreateOrthographicOffCenter(
                    0,
                    graphicsDevice.Viewport.Width,
                    graphicsDevice.Viewport.Height,
                    0, 1, 0);

                if (effect is BasicEffect)
                {
                    ((BasicEffect)SkeletonRenderer.Effect).Projection = projectionMatrix;
                }
                else
                {
                    SkeletonRenderer.Effect.Parameters["Projection"].SetValue(projectionMatrix);
                }
            }

        }
    }
}
