using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(DebugHeaderAttribute))]
    public class DebugHeaderPropertyDrawer : AbstractTitleHeader
    {
        public override GUIContent CreateContent()
        {
            return new GUIContent("Debug", "The following fields are meant for debugging only");
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class DebugHeaderAttribute : PropertyAttribute
    {
    }
}