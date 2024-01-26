using FlatRedBall.Glue.CodeGeneration;
using FlatRedBall.Glue.Elements;
using FlatRedBall.Glue.SaveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinePlugin.Managers
{
    internal static class AssetTypeInfoManager
    {
        static AssetTypeInfo atlasAssetTypeInfo;
        
        public static AssetTypeInfo AtlasAssetTypeInfo =>
            atlasAssetTypeInfo = atlasAssetTypeInfo ?? CreateAtlasAssetTypeInfo();

        static AssetTypeInfo spineDrawableBatchAssetTypeInfo;
        public static AssetTypeInfo SpineDrawableBatchAssetTypeInfo =>
            spineDrawableBatchAssetTypeInfo = spineDrawableBatchAssetTypeInfo ?? CreateSpineDrawableBatchAssetTypeInfo();

        static AssetTypeInfo CreateAtlasAssetTypeInfo()
        {
            var ati = new AssetTypeInfo();

            ati.QualifiedRuntimeTypeName = new PlatformSpecificType
            {
                QualifiedType = "Spine.Atlas"
            };
            ati.Extension = "atlas";
            ati.FriendlyName = "Spine Atlas";
            ati.CustomLoadFunc = CustomLoadAtlasFunc;

            return ati;
        }

        private static string CustomLoadAtlasFunc(IElement element, NamedObjectSave save1, ReferencedFileSave rfs, string contentManager)
        {
            var file = ReferencedFileSaveCodeGenerator.GetFileToLoadForRfs(rfs);
            return $"{rfs.GetInstanceName()} = " +
                $"new global::Spine.Atlas(\"{file}\", new global::FlatRedBall.Spine.FrbSpineTextureLoader({contentManager}));";

        }

        private static AssetTypeInfo CreateSpineDrawableBatchAssetTypeInfo()
        {
            var ati = new AssetTypeInfo();
            ati.QualifiedRuntimeTypeName = new PlatformSpecificType
            {
                QualifiedType = "FlatRedBall.Spine.SpineDrawableBatch"
            };
            ati.Extension = "json";
            ati.FriendlyName = "SpineDrawableBatch";
            ati.CustomLoadFunc = CustomLoadSpineDrawableBatchFunc;
            ati.AddToManagersFunc = AddToManagersSpineDrawableBatchFunc;
            ati.DestroyFunc = DestroySpineDrawableBatchFunc;
            ati.IsPositionedObject = true;
            ati.CanBeObject = true;
            ati.ShouldAttach = true;
            ati.CanBeCloned = true;

            ati.VariableDefinitions.Add(new VariableDefinition
            {
                Name = "ScaleX",
                Type = "float",
                DefaultValue = "1",
                CanBeSetInFile = false,
            });

            ati.VariableDefinitions.Add(new VariableDefinition
            {
                Name = "ScaleY",
                Type = "float",
                DefaultValue = "1",
                CanBeSetInFile = false,
            });


            return ati;
        }

        private static string CustomLoadSpineDrawableBatchFunc(IElement element, NamedObjectSave save1, ReferencedFileSave rfs, string arg4)
        {
            var atlasProperty = rfs.GetProperty<string>("AtlasName") ?? "no atlas";

            var file = ReferencedFileSaveCodeGenerator.GetFileToLoadForRfs(rfs);

            return $"{rfs.GetInstanceName()} = FlatRedBall.Spine.SpineDrawableBatch.FromFile(\"{file}\", {atlasProperty});";
        }

        private static string AddToManagersSpineDrawableBatchFunc(IElement element, NamedObjectSave nos, ReferencedFileSave rfs, string arg4)
        {
            if (rfs != null)
            {
                return $"{rfs.GetInstanceName()}.AddToManagers(null);";
            }
            else
            {
                return $"{nos.FieldName}.AddToManagers(null);";
            }

        }

        private static string DestroySpineDrawableBatchFunc(IElement element, NamedObjectSave nos, ReferencedFileSave rfs)
        {
            if (rfs != null)
            {
                return $"FlatRedBall.SpriteManager.RemoveDrawableBatch({rfs.GetInstanceName()});";
            }
            else
            {
                return $"FlatRedBall.SpriteManager.RemoveDrawableBatch({nos.FieldName});";
            }

        }
    }
}
