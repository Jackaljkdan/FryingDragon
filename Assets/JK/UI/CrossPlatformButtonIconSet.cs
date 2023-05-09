using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UI
{
    [CreateAssetMenu(fileName = "CrossPlatformButtonIconSet", menuName = "JK/UI/CrossPlatformButtonIconSet")]
    public class CrossPlatformButtonIconSet : ScriptableObject
    {
        #region Inspector fields

        public ButtonIconSet keyboard;
        public ButtonIconSet controller;

        #endregion

        public ButtonIconSet Get(InputType type)
        {
            switch (type)
            {
                default:
                case InputType.Keyboard:
                    return keyboard;
                case InputType.Controller:
                    return controller;
            }
        }
    }
}