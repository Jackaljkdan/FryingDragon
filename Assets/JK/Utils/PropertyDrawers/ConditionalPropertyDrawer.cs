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

    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalPropertyDrawer : PropertyDrawer
    {
        private bool ShouldDraw(SerializedProperty property)
        {
            if (!(attribute is ConditionalAttribute conditionalAttribute))
                return true;

            var conditionProperty = property.serializedObject.FindProperty(conditionalAttribute.conditionPropertyName);

            if (conditionProperty == null)
                return true;

            bool conditionValue = GetBool(conditionProperty);

            if (!conditionalAttribute.inverse)
                return conditionValue;
            else
                return !conditionValue;
        }

        private bool GetBool(SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    return property.boolValue;
                case SerializedPropertyType.Integer:
                    return property.intValue != 0;
                case SerializedPropertyType.Float:
                    return property.floatValue != 0;
                case SerializedPropertyType.ArraySize:
                    return property.arraySize != 0;
                case SerializedPropertyType.String:
                case SerializedPropertyType.Character:
                    return !string.IsNullOrEmpty(property.stringValue);
                case SerializedPropertyType.ExposedReference:
                    return property.exposedReferenceValue != null;
                case SerializedPropertyType.ManagedReference:
                    return property.managedReferenceValue != null;
                case SerializedPropertyType.ObjectReference:
                    return property.objectReferenceValue != null;
                default:
                    return true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldDraw(property))
                return EditorGUI.GetPropertyHeight(property, label, true);
            else
                return 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldDraw(property))
                EditorGUI.PropertyField(position, property, label, true);
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public readonly string conditionPropertyName;

        public readonly bool inverse;

        public ConditionalAttribute(string conditionPropertyName, bool inverse = false)
        {
            this.conditionPropertyName = conditionPropertyName;
            this.inverse = inverse;
        }
    }
}