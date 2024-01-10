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


            var bbcode = "Hi [Font=Bauhaus 93]this is[/Font] some [Color=Purple]purple[/Color] text";
            var tags = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "red", "blue", "green", "color", "font" };

            var results = BbCodeParser.Parse(tags, bbcode);
            var strippedText = BbCodeParser.RemoveTags(bbcode, results);
            GumScreen.TextInstance.Text = strippedText;

            foreach(var item in results)
            {
                object castedValue = item.Open.Argument;
                string convertedName = item.Name;
                switch(item.Name)
                {
                    case "Red":
                    case "Green":
                    case "Blue":
                        castedValue = byte.Parse(item.Open.Argument);
                        break;
                    case "Color":
                        {
                            int result;

                            if (item.Open.Argument?.StartsWith("0x") == true && int.TryParse(item.Open.Argument.Substring(2),
                                                                                NumberStyles.AllowHexSpecifier,
                                                                                null,
                                                                                out result))
                            {
                                castedValue = result;
                                castedValue = System.Drawing.Color.FromArgb(result);
                            }
                            else
                            {
                                castedValue = System.Drawing.Color.FromName(item.Open.Argument);
                            }
                        }
                        break;


                }

                textRenderable.InlineVariables.Add(new InlineVariable
                {
                    CharacterCount = item.Close.StartStrippedIndex - item.Open.StartStrippedIndex,
                    StartIndex = item.Open.StartStrippedIndex,
                    VariableName = convertedName,
                    Value = castedValue
                });
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
