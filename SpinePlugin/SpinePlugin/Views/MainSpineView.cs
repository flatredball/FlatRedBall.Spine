using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfDataUi;

namespace Spine.Plugin.Views
{
    internal class MainSpineView : UserControl
    {
        // Vic says - I couldn't get the XAML to load. No idea why, searched tons of posts, nothing answered it.
        // I kept getting this:
        /*
         * 7:54:54 PM - Spine Plugin Version 1.0 Failed in ReactToItemsSelected
System.IO.IOException: Cannot locate resource 'spinetabcontrol.xaml'.
   at MS.Internal.AppModel.ResourcePart.GetStreamCore(FileMode mode, FileAccess access)
   at System.IO.Packaging.PackagePart.GetStream(FileMode mode, FileAccess access)
   at MS.Internal.IO.Packaging.PackagePartExtensions.GetSeekableStream(PackagePart packPart, FileMode mode, FileAccess access)
   at MS.Internal.IO.Packaging.PackagePartExtensions.GetSeekableStream(PackagePart packPart)
   at System.Windows.Application.LoadComponent(Object component, Uri resourceLocator)
   at Spine.Plugin.SpineTabControl.InitializeComponent()
   at Spine.Plugin.SpineTabControl..ctor()
   at SpinePlugin.MainSpinePlugin.CreateUi()
   at SpinePlugin.MainSpinePlugin.HandleItemSelected(List`1 list)
   at FlatRedBall.Glue.Plugins.PluginManager.<>c__DisplayClass90_0.<ReactToItemsSelected>b__0(PluginBase plugin) in C:\Users\vchel\Documents\GitHub\FlatRedBall\FRBDK\Glue\Glue\Plugins\PluginManager.cs:line 1256
   at FlatRedBall.Glue.Plugins.PluginManager.<>c__DisplayClass130_1.<CallMethodOnPlugin>b__3() in C:\Users\vchel\Documents\GitHub\FlatRedBall\FRBDK\Glue\Glue\Plugins\PluginManager.cs:line 1835
   at FlatRedBall.Glue.Plugins.PluginManager.<>c__DisplayClass134_0.<PluginCommand>b__0() in C:\Users\vchel\Documents\GitHub\FlatRedBall\FRBDK\Glue\Glue\Plugins\PluginManager.cs:line 1976
         * 
         * 
         * 
         * 
         */

        // So I'm going to just do it manually:
        // Update I believe it was caused by this, so we could go back to XAML if there's any benefit
        // https://github.com/vchelaru/FlatRedBall/issues/1323

        public DataUiGrid DataGrid { get; private set; }

        public MainSpineView()
        {
            var grid = new Grid();
            this.Content = grid;

            DataGrid = new DataUiGrid();
            grid.Children.Add(DataGrid);

        }

    }
}
