using System;
using UnityEditor;
using UnityEngine;

namespace JK.Utils.Editor
{
    [CustomPropertyDrawer(typeof(IconBoxAttribute), useForChildren: true)]
    public class IconBoxHeader : PropertyDrawer
    {
        public static readonly float marginVertical = 8;
        public static readonly float boxHeight = EditorGUIUtility.singleLineHeight * 3;

        public DebouncedMethodCaller<EditorMessageArgs> debouncedMethod;

        public IconBoxAttribute IconBoxAttribute => (IconBoxAttribute)attribute;

        private EditorMessageArgs GetBoxArgs(SerializedProperty property)
        {
            if (debouncedMethod == null)
                debouncedMethod = new DebouncedMethodCaller<EditorMessageArgs>(property.serializedObject.targetObject, IconBoxAttribute.methodName, IconBoxAttribute.debounceSeconds);

            if (!debouncedMethod.IsValid)
                return default;

            return debouncedMethod.InvokeIfAppropriateT();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            EditorMessageArgs boxArgs = GetBoxArgs(property);

            float propertyHeight = IconBoxAttribute.hideField ? 0 : EditorGUI.GetPropertyHeight(property, label, includeChildren: true);

            if (boxArgs.show)
                return boxHeight + propertyHeight;
            else
                return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorMessageArgs boxArgs = GetBoxArgs(property);

            if (boxArgs.show)
            {
                Rect boxPosition = position;
                boxPosition.height = boxHeight - marginVertical * 2;
                boxPosition.y += marginVertical;
                EditorGUI.HelpBox(boxPosition, boxArgs.message ?? IconBoxAttribute.title, boxArgs.type.Convert());

                Rect propertyPosition = position;
                propertyPosition.height -= boxHeight;
                propertyPosition.y += boxHeight;

                if (!IconBoxAttribute.hideField)
                    EditorGUI.PropertyField(propertyPosition, property, label, includeChildren: true);
            }
            else if (!IconBoxAttribute.hideField)
            {
                EditorGUI.PropertyField(position, property, label, includeChildren: true);
            }
        }
    }
}