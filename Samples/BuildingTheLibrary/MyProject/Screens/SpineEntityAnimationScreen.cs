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
using FlatRedBall.Spine;
using FlatRedBall.Forms.Controls;
using Spine;


namespace MyProject.Screens
{
    public partial class SpineEntityAnimationScreen
    {

        void CustomInitialize()
        {
            if (!SpineManager.IsInitialized)
            {
                SpineManager.Initialize(FlatRedBallServices.GraphicsDevice, GlobalContent.SpineEffect);
            }

            var stackLayout = new FlatRedBall.Forms.Controls.StackPanel();
            stackLayout.Visual.AddToManagers();
            foreach (var animation in SpineEntityInstance.SpineDrawableBatch.AnimationState.Data.SkeletonData.Animations)
            {
                var button = new Button();
                button.Text = animation.Name;
                stackLayout.AddChild(button);
                button.Click += (_, _) =>
                {
                    SpineEntityInstance.SpineDrawableBatch.PlayAnimation(animation.Name);

                };
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
