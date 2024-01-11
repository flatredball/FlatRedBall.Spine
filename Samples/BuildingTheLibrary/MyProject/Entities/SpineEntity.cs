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

        SpineDrawableBatch spineDrawableBatch;
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            if(!SpineManager.IsInitialized)
            {
                SpineManager.Initialize(FlatRedBallServices.GraphicsDevice, SpineEffect);
            }

            spineDrawableBatch = SpineDrawableBatch.FromFile(
                assetsFolder + "spineboy-ess.json", assetsFolder + "spineboy.atlas");

            spineDrawableBatch.AddToManagers(null);
            spineDrawableBatch.AttachTo(this);
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
