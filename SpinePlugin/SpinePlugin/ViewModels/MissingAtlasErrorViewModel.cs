using FlatRedBall.Glue.Errors;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using FlatRedBall.Glue.SaveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spine.Plugin.ViewModels
{
    public class MissingAtlasErrorViewModel : ErrorViewModel
    {
        public override string UniqueId => Details;

        public GlueElement? Element { get; }
        public ReferencedFileSave ReferencedFileSave { get; }

        public MissingAtlasErrorViewModel(GlueElement? element, ReferencedFileSave rfs)
        {
            this.Element = element;
            this.ReferencedFileSave = rfs;

            this.Details = $"Missing atlas for {rfs}";
        }

        public override void HandleDoubleClick()
        {
            GlueState.Self.CurrentReferencedFileSave = ReferencedFileSave;
        }

        public override bool GetIfIsFixed()
        {
            return !GetIfHasError(Element, ReferencedFileSave);
        }

        public static bool GetIfHasError(GlueElement? element, ReferencedFileSave rfs)
        {
            if( (element == null || element.ReferencedFiles.Contains(rfs)) && 
                rfs.GetAssetTypeInfo() == SpinePlugin.Managers.AssetTypeInfoManager.SpineDrawableBatchAssetTypeInfo)
            {
                var atlasProperty = rfs.GetProperty<string>("AtlasName");

                if(string.IsNullOrEmpty(atlasProperty))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
