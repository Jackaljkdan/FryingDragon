using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(IconFieldAttribute))]
    public class IconFieldPropertyDrawer : PropertyDrawer
    {
        public IconFieldAttribute IconAttribute => (IconFieldAttribute)attribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float iconMargin = 4;

            Rect iconPosition = position;
            iconPosition.width = position.height;

            var tex = EditorGUIUtility.FindTexture(IconAttribute.iconPath);

            if (tex != null)
                EditorGUI.DrawPreviewTexture(iconPosition, tex);

            position.width -= iconPosition.width;
            position.x += iconPosition.width + iconMargin;

            EditorGUI.PropertyField(position, property, label, includeChildren: true);

            EditorGUI.EndProperty();
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class IconFieldAttribute : PropertyAttribute
    {
        public string iconPath;

        public IconFieldAttribute(string iconPath)
        {
            this.iconPath = iconPath;
        }
    }
}