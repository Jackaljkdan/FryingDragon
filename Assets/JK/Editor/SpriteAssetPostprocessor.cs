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
        void OnPreprocessTexture()
        {
            // preprocess only when imported the first time
            if (!assetImporter.importSettingsMissing)
                return;

            if (assetImporter is not TextureImporter textureImporter)
                return;

            bool isInSpritesFolder = assetPath.ToLowerInvariant().Contains("/sprites");

            if (!isInSpritesFolder)
                return;

            var settings = new TextureImporterSettings();

            textureImporter.ReadTextureSettings(settings);

            settings.textureType = TextureImporterType.Sprite;
            settings.alphaIsTransparency = true;
            settings.spriteMode = (int)SpriteImportMode.Single;
            settings.spriteMeshType = SpriteMeshType.FullRect;

            textureImporter.SetTextureSettings(settings);
        }
    }
}