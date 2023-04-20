using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(RuntimeFieldAttribute))]
    public class RuntimeFieldPropertyDrawer : IconFieldPropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = Application.isPlaying;
            base.OnGUI(position, property, label);
            GUI.enabled = true;
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class RuntimeFieldAttribute : IconFieldAttribute
    {
        public RuntimeFieldAttribute() : base("Assets/JK/Utils/PropertyDrawers/Sprites/Runtime.png") { }
    }
}