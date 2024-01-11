using FlatRedBall.Glue.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinePlugin
{
    [System.ComponentModel.Composition.Export(typeof(FlatRedBall.Glue.Plugins.PluginBase))]
    public class MainSpinePlugin : PluginBase
    {
        public override string FriendlyName => "Spine Plugin";

        public override void StartUp()
        {
            
        }
    }
}
