using Microsoft.Xna.Framework.Graphics;
using Spine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatRedBall.Spine
{
    public class FrbSpineTextureLoader : TextureLoader
    {
        string[] textureLayerSuffixes = null;
        public string ContentManagerName { get; set; }

        public FrbSpineTextureLoader(string contentManagerName, bool loadMultipleTextureLayers = false, string[] textureSuffixes = null)
        {
            //this.device = device;
            if (loadMultipleTextureLayers)
                this.textureLayerSuffixes = textureSuffixes;
        }

        public void Load(AtlasPage page, String path)
        {
            Texture2D texture = FlatRedBallServices.Load<Texture2D>(path, ContentManagerName);
            page.width = texture.Width;
            page.height = texture.Height;

            if (textureLayerSuffixes == null)
            {
                page.rendererObject = texture;
            }
            else
            {
                Texture2D[] textureLayersArray = new Texture2D[textureLayerSuffixes.Length];
                textureLayersArray[0] = texture;
                for (int layer = 1; layer < textureLayersArray.Length; ++layer)
                {
                    string layerPath = GetLayerName(path, textureLayerSuffixes[0], textureLayerSuffixes[layer]);
                    textureLayersArray[layer] =
                        FlatRedBallServices.Load<Texture2D>(layerPath, ContentManagerName);
                }
                page.rendererObject = textureLayersArray;
            }
        }

        public void Unload(Object texture)
        {
            // I don't think we should ever do this, these textures are shared and the content manager handles it...
            //((Texture2D)texture).Dispose();
        }

        private string GetLayerName(string firstLayerPath, string firstLayerSuffix, string replacementSuffix)
        {

            int suffixLocation = firstLayerPath.LastIndexOf(firstLayerSuffix + ".");
            if (suffixLocation == -1)
            {
                throw new Exception(string.Concat("Error composing texture layer name: first texture layer name '", firstLayerPath,
                                "' does not contain suffix to be replaced: '", firstLayerSuffix, "'"));
            }
            return firstLayerPath.Remove(suffixLocation, firstLayerSuffix.Length).Insert(suffixLocation, replacementSuffix);
        }
    }
}
