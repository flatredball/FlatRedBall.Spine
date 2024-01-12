using FlatRedBall.Graphics;
using FlatRedBall.IO;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRedBall.Spine
{
    public class SpineDrawableBatch : PositionedObject, IDrawableBatch
    {

        Skeleton skeleton;
        AnimationState state;
        //Slot headSlot;

        SkeletonBounds bounds = new SkeletonBounds();

        public bool UpdateEveryFrame => false;

        /// <summary>
        /// Don't call this, instead call SpriteManager.RemoveDrawableBatch
        /// </summary>
        public void Destroy()
        {
            this.RemoveSelfFromListsBelongingTo();
        }

        public static SpineDrawableBatch FromFile(FilePath skeletonFile, FilePath atlasFile)
        {
            return FromFile(skeletonFile.FullPath, atlasFile.FullPath);
        }

        public static SpineDrawableBatch FromFile(string skeletonPath, string atlasPath)
        {
            Atlas atlas = new Atlas(atlasPath, new XnaTextureLoader(FlatRedBallServices.GraphicsDevice));
            return FromFile(skeletonPath, atlas);
        }

        public static SpineDrawableBatch FromFile(string skeletonPath, Atlas atlas)
        { 
            SpineDrawableBatch toReturn = new SpineDrawableBatch();


            float scale = 1 / 8f;

            SkeletonData skeletonData;

            var skeletonExtension = FileManager.GetExtension(skeletonPath);

            if (skeletonExtension == "skel")
            {
                SkeletonBinary binary = new SkeletonBinary(atlas);
                binary.Scale = scale;
                skeletonData = binary.ReadSkeletonData(skeletonPath);
            }
            else
            {
                SkeletonJson json = new SkeletonJson(atlas);
                json.Scale = scale;
                skeletonData = json.ReadSkeletonData(skeletonPath);
            }
            toReturn.skeleton = new Skeleton(skeletonData);

            // set skin?
            //if (name == "goblins-pro") skeleton.SetSkin("goblin");

            // Define mixing between animations.
            AnimationStateData stateData = new AnimationStateData(toReturn.skeleton.Data);
            toReturn.state = new AnimationState(stateData);

            //if (name == "spineboy-ess")
            //{
            //    skeleton.SetAttachment("head-bb", "head"); // Activate the head BoundingBoxAttachment.

            //    stateData.SetMix("run", "jump", 0.2f);
            //    stateData.SetMix("jump", "run", 0.4f);

            //    // Event handling for all animations.
            //    state.Start += Start;
            //    state.End += End;
            //    state.Complete += Complete;
            //    state.Event += Event;

            //    state.SetAnimation(0, "run", false);
            //    TrackEntry entry = state.AddAnimation(0, "jump", false, 0);
            //    entry.End += End; // Event handling for queued animations.
            //    state.AddAnimation(0, "run", true, 0);
            //}
            //else if (name == "raptor-pro")
            //{
            //state.SetAnimation(0, "walk", true);
            //state.AddAnimation(1, "gun-grab", false, 2);
            //}
            //else if (name == "coin-pro")
            //{
            //    state.SetAnimation(0, "animation", true);
            //}
            //else if (name == "tank-pro")
            //{
            //    state.SetAnimation(0, "drive", true);
            //}
            //else
            //{
            //    state.SetAnimation(0, "walk", true);
            //}

            toReturn.skeleton.ScaleY = -1;

            //headSlot = skeleton.FindSlot("head");
            return toReturn;
        }

        public void Draw(Camera camera)
        {
            state.Update(TimeManager.SecondDifference);
            state.Apply(skeleton);
            skeleton.UpdateWorldTransform();
            skeleton.X = 2 * (X) + Camera.Main.OrthogonalWidth;
            skeleton.Y = (-Y * 2) + Camera.Main.OrthogonalHeight;

            //SpineEffect.Parameters["World"].SetValue(Matrix.Identity);

            SpineManager.PrepareDraw();
            var skeletonRenderer = SpineManager.SkeletonRenderer;

            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();

            bounds.Update(skeleton, true);
            //MouseState mouse = Mouse.GetState();
            //if (headSlot != null)
            //{
            //headSlot.G = 1;
            //headSlot.B = 1;
            //if (bounds.AabbContainsPoint(mouse.X, mouse.Y))
            //{
            //    BoundingBoxAttachment hit = bounds.ContainsPoint(mouse.X, mouse.Y);
            //    if (hit != null)
            //    {
            //        headSlot.G = 0;
            //        headSlot.B = 0;
            //    }
            //}
            //}
        }

        public void Update()
        {

        }

        public void AddToManagers(Layer layer)
        {
            bool isAlreadyManaged = SpriteManager.ManagedPositionedObjects
                .Contains(this);

            // This allows AddToManagers to be called multiple times, so it can be added to multiple layers
            if (!isAlreadyManaged)
            {
                SpriteManager.AddPositionedObject(this);
            }

            SpriteManager.AddToLayer(this, layer);
        }

        public SpineDrawableBatch Clone()
        {
            var clone = new SpineDrawableBatch();
            clone.skeleton = skeleton;
            AnimationStateData stateData = new AnimationStateData(clone.skeleton.Data);
            clone.state = new AnimationState(stateData);
            return clone;
        }
    }
}
