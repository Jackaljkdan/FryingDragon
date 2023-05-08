using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor
{
    public class SpriteAssetPostprocessor : AssetPostprocessor
    {
        void OnPreprocessTexture(Texture2D texture)
        {
            // preprocess only when imported the first time
            if (!assetImporter.importSettingsMissing)
                return;

            if (assetImporter is not TextureImporter textureImporter)
                return;

            bool isInSpritesFolder = assetPath.ToLowerInvariant().Contains("/sprites");

            if (!isInSpritesFolder)
                return;

            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.alphaIsTransparency = true;
        }
    }
}