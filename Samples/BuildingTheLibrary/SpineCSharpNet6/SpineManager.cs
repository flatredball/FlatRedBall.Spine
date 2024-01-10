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

        public static void PrepareDraw()
        {
            var zoom = Camera.Main.DestinationRectangle.Height / Camera.Main.OrthogonalHeight;

            var effect = SkeletonRenderer.Effect;
            effect.Parameters["World"].SetValue(Matrix.CreateTranslation(-Camera.Main.X * zoom, Camera.Main.Y * zoom, 0));
            effect.Parameters["View"].SetValue(Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up));

            var graphicsDevice = FlatRedBallServices.GraphicsDevice;

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
