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
using FlatRedBall.Spine;




namespace MyProject.Screens
{
    public partial class GameScreen
    {

        void CustomInitialize()
        {

            if (!SpineManager.IsInitialized)
            {
                SpineManager.Initialize(FlatRedBallServices.GraphicsDevice, GlobalContent.SpineEffect);
            }
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

    }
}
