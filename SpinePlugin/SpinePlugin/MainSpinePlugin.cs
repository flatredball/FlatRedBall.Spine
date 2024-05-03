using FlatRedBall;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.FormHelpers;
using FlatRedBall.Glue.Plugins;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.SaveClasses;
using FlatRedBall.Graphics.Texture;
using Spine.Plugin;
using Spine.Plugin.Views;
using SpinePlugin.Managers;
using SpinePlugin.ViewModels;
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

        public override Version Version => new Version(0,3,0,0);

        MainSpineView MainSpineControl;

        PluginTab MainTab;

        public override void StartUp()
        {
            AssignEvents();

            CreateAssetTypeInfos();
        }

        bool hasCreatedUi = false;
        private void CreateUi()
        {
            hasCreatedUi = true;
            MainSpineControl = new MainSpineView();
            MainTab = CreateTab(MainSpineControl, "Spine");
        }

        private void AssignEvents()
        {
            this.ReactToItemsSelected += HandleItemSelected;
            this.AddEventsForObject += EventManager.HandleAddEventsForObject;
            this.GetEventSignatureArgs += EventManager.HandleGetEventSignatureArgs;
        }

        private void HandleItemSelected(List<ITreeNode> list)
        {
            if(!hasCreatedUi)
            {
                CreateUi();
            }
            var rfs = GlueState.Self.CurrentReferencedFileSave;
            var ati = rfs?.GetAssetTypeInfo();

            if(ati != AssetTypeInfoManager.SpineDrawableBatchAssetTypeInfo)
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
                if (element != null)
                {
                    foreach (var possibleAtlas in element.ReferencedFiles)
                    {
                        if (possibleAtlas.GetAssetTypeInfo() == AssetTypeInfoManager.AtlasAssetTypeInfo)
                        {
                            customOptions.Add(possibleAtlas.GetInstanceName());
                        }
                    }
                }

                foreach (var possibleAtlas in GlueState.Self.CurrentGlueProject.GlobalFiles)
                {
                    if (possibleAtlas.GetAssetTypeInfo() == AssetTypeInfoManager.AtlasAssetTypeInfo)
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
            this.AddAssetTypeInfo(AssetTypeInfoManager.SpineDrawableBatchAssetTypeInfo);
            this.AddAssetTypeInfo(AssetTypeInfoManager.AtlasAssetTypeInfo);
        }

    }
}
