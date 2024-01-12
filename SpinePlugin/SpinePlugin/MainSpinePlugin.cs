using FlatRedBall;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.FormHelpers;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Graphics.Texture;
using SpinePlugin.Managers;
using SpinePlugin.ViewModels;
using SpinePlugin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDataUi.DataTypes;

namespace SpinePlugin
{
    [System.ComponentModel.Composition.Export(typeof(FlatRedBall.Glue.Plugins.PluginBase))]
    public class MainSpinePlugin : PluginBase
    {
        public override string FriendlyName => "Spine Plugin";

        MainSpineControl MainSpineControl;

        PluginTab MainTab;

        public override void StartUp()
        {
            AssignEvents();

            CreateAssetTypeInfos();

            CreateUi();
        }

        private void CreateUi()
        {
            MainSpineControl = new MainSpineControl();
            MainTab = CreateTab(MainSpineControl, "Spine");
        }

        private void AssignEvents()
        {
            this.ReactToItemsSelected += HandleItemSelected;
        }

        private void HandleItemSelected(List<ITreeNode> list)
        {
            var rfs = GlueState.Self.CurrentReferencedFileSave;

            if(rfs == null)
            {
                MainTab.Hide();
            }
            else
            {
                var viewModel = new MainSpineControlViewModel();
                viewModel.GlueObject = rfs;
                viewModel.UpdateFromGlueObject();

                var dataGrid = MainSpineControl.DataGrid;
                dataGrid.Instance = viewModel;

                var category = dataGrid.Categories[0];

                category.Members.Clear();

                var atlasMember = new InstanceMember(nameof(viewModel.AtlasName), viewModel);

                List<object> customOptions = new List<object>();

                // add all the names of items that have the atlas type
                var element = GlueState.Self.CurrentElement;
                if(element != null)
                {
                    foreach(var possibleAtlas in element.ReferencedFiles)
                    {
                        if(possibleAtlas.GetAssetTypeInfo() == AssetTypeInfoManager.AtlasAssetTypeInfo)
                        {
                            customOptions.Add(possibleAtlas.GetInstanceName());
                        }
                    }
                }

                foreach(var possibleAtlas in GlueState.Self.CurrentGlueProject.GlobalFiles)
                {
                    if(possibleAtlas.GetAssetTypeInfo() == AssetTypeInfoManager.AtlasAssetTypeInfo)
                    {
                        customOptions.Add("GlobalContent." + possibleAtlas.GetInstanceName());
                    }
                }

                atlasMember.CustomOptions = customOptions;

                category.Members.Add(atlasMember);

                MainTab.Show();


            }
        }

        private void CreateAssetTypeInfos()
        {
            this.AddAssetTypeInfo(AssetTypeInfoManager.CreateSpineDrawableBatchAssetTypeInfo());
            this.AddAssetTypeInfo(AssetTypeInfoManager.AtlasAssetTypeInfo);
        }

    }
}
