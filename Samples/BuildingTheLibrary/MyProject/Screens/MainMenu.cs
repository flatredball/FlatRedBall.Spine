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
using RenderingLibrary.Graphics;
using System.Globalization;
using Gum.Wireframe;


namespace MyProject.Screens
{
    public partial class MainMenu
    {

        void CustomInitialize()
        {
            var text = GumScreen.TextInstance;
            var textRenderable = text.Component as Text;

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
