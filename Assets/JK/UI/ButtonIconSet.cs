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

        public Sprite primary;
        public Sprite secondary;
        public Sprite tertiary;
        public Sprite quaternary;

        #endregion

        public Sprite Get(ButtonType type)
        {
            switch (type)
            {
                case ButtonType.Primary:
                default:
                    return primary;
                case ButtonType.Secondary:
                    return secondary;
                case ButtonType.Tertiary:
                    return tertiary;
                case ButtonType.Quaternary:
                    return quaternary;
            }
        }
    }
}