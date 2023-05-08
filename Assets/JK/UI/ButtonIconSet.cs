using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace JK.UI
{
    [CreateAssetMenu(fileName = "ButtonIconSet", menuName = "JK/UI/ButtonIconSet")]
    public class ButtonIconSet : ScriptableObject
    {
        #region Inspector fields

        public AssetReferenceSprite primaryAsset;
        public AssetReferenceSprite secondaryAsset;
        public AssetReferenceSprite tertiaryAsset;
        public AssetReferenceSprite quaternaryAsset;

        #endregion

        public AssetReferenceT<Sprite> Get(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.Primary:
                default:
                    return primaryAsset;
                case ButtonType.Secondary:
                    return secondaryAsset;
                case ButtonType.Tertiary:
                    return tertiaryAsset;
                case ButtonType.Quaternary:
                    return quaternaryAsset;
            }
        }
    }
}