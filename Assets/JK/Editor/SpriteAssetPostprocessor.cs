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
        void OnPostprocessTexture(Texture2D texture)
        {
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