using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using Spine;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using FlatRedBall.Spine;
using FlatRedBall.IO;

namespace MyProject.Entities
{
    public partial class SpineEntity
    {
        private string assetsFolder = "content/Spine/data/";
        Skeleton skeleton;
        AnimationState state;
        //Slot headSlot;

        SkeletonBounds bounds = new SkeletonBounds();

        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            LoadSpineFile(assetsFolder + "spineboy-ess.json", assetsFolder + "spineboy.atlas");
        }

        void LoadSpineFile(FilePath skeletonFile, FilePath atlasFile)
        { 
            if(!SpineManager.IsInitialized)
            {
                SpineManager.Initialize(FlatRedBallServices.GraphicsDevice, SpineEffect);
            }
            
            var name = skeletonFile.NoPathNoExtension;
            Atlas atlas = new Atlas(atlasFile.FullPath, new XnaTextureLoader(FlatRedBallServices.GraphicsDevice));

            float scale = 1/8f;
            
            SkeletonData skeletonData;



            if (skeletonFile.Extension == "skel")
            {
                SkeletonBinary binary = new SkeletonBinary(atlas);
                binary.Scale = scale;
                skeletonData = binary.ReadSkeletonData(skeletonFile.FullPath);
            }
            else
            {
                SkeletonJson json = new SkeletonJson(atlas);
                json.Scale = scale;
                skeletonData = json.ReadSkeletonData(skeletonFile.FullPath);
            }
            skeleton = new Skeleton(skeletonData);

            // set skin?
            //if (name == "goblins-pro") skeleton.SetSkin("goblin");

            // Define mixing between animations.
            AnimationStateData stateData = new AnimationStateData(skeleton.Data);
            state = new AnimationState(stateData);

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

            skeleton.ScaleY = -1;

            //headSlot = skeleton.FindSlot("head");
        }

        private void CustomActivity()
        {


        }

        private void CustomDestroy()
        {


        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


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


        //public void Start(TrackEntry entry)
        //{
        //    Console.WriteLine(entry + ": start");
        //}

        //public void End(TrackEntry entry)
        //{
        //    Console.WriteLine(entry + ": end");
        //}

        //public void Complete(TrackEntry entry)
        //{
        //    Console.WriteLine(entry + ": complete ");
        //}

        //public void Event(TrackEntry entry, Event e)
        //{
        //    Console.WriteLine(entry + ": event " + e);
        //}

        // other code that may be helpful in the future:
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
    }
}
