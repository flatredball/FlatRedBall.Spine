using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Gui;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Localization;
using Microsoft.Xna.Framework;

using MyProject.Entities;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata;
using Spine;
using System.Collections;
using FlatRedBall.Graphics;
using FlatRedBall.Glue.StateInterpolation;
using Microsoft.Xna.Framework.Input;


namespace MyProject.Screens
{
    public partial class Level1 
    {
        Entities.SpineEntity splineEntity;
        void CustomInitialize()
        {
            splineEntity = Factories.SpineEntityFactory.CreateNew(00, -500, 0);
        }


        void CustomActivity(bool firstTimeCalled)
        {

        }

        void CustomDestroy()
        {


        }

        

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        float x = 0;
        public void Draw(Camera camera)
        {
            

        }

        public void Update()
        {

        }
    }
}
