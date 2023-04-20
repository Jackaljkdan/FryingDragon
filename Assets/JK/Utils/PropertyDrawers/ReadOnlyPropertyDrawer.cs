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

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public DebouncedMethodCaller debouncedMethod;

        public ReadOnlyAttribute ReadOnlyAttribute => (ReadOnlyAttribute)attribute;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (debouncedMethod == null)
                debouncedMethod = new DebouncedMethodCaller(property.serializedObject.targetObject, ReadOnlyAttribute.callbackMethodName, ReadOnlyAttribute.debounceSeconds);

            debouncedMethod.InvokeIfAppropriate();

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, includeChildren: true);
            GUI.enabled = true;
        }
    }

#endif

    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public string callbackMethodName;
        public float debounceSeconds = 0.1f;

        public ReadOnlyAttribute() { }

        public ReadOnlyAttribute(string callbackMethodName)
        {
            this.callbackMethodName = callbackMethodName;
        }

        public ReadOnlyAttribute(string callbackMethodName, float debounceSeconds)
        {
            this.callbackMethodName = callbackMethodName;
            this.debounceSeconds = debounceSeconds;
        }
    }
}