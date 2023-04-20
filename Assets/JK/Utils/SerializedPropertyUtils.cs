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
    public static class SerializedPropertyUtils
    {
#if UNITY_EDITOR

        public static object GetValueBasedOnType(this SerializedProperty self)
        {
            switch (self.propertyType)
            {
                case SerializedPropertyType.Generic:
                default:
                    return null;
                case SerializedPropertyType.Enum:
                case SerializedPropertyType.Integer:
                    return self.intValue;
                case SerializedPropertyType.Boolean:
                    return self.boolValue;
                case SerializedPropertyType.Float:
                    return self.floatValue;
                case SerializedPropertyType.String:
                    return self.stringValue;
                case SerializedPropertyType.Color:
                    return self.colorValue;
                case SerializedPropertyType.ObjectReference:
                    return self.objectReferenceValue;
                case SerializedPropertyType.LayerMask:
                    return (LayerMask)self.intValue;
                case SerializedPropertyType.Vector2:
                    return self.vector2Value;
                case SerializedPropertyType.Vector3:
                    return self.vector3Value;
                case SerializedPropertyType.Vector4:
                    return self.vector4Value;
                case SerializedPropertyType.Rect:
                    return self.rectValue;
                case SerializedPropertyType.ArraySize:
                    return self.arraySize;
                case SerializedPropertyType.Character:
                    return self.stringValue;
                case SerializedPropertyType.AnimationCurve:
                    return self.animationCurveValue;
                case SerializedPropertyType.Bounds:
                    return self.boundsValue;
                case SerializedPropertyType.Gradient:
                    return self.GetGradient();
                case SerializedPropertyType.Quaternion:
                    return self.quaternionValue;
                case SerializedPropertyType.ExposedReference:
                    return self.exposedReferenceValue;
                case SerializedPropertyType.FixedBufferSize:
                    return self.fixedBufferSize;
                case SerializedPropertyType.Vector2Int:
                    return self.vector2IntValue;
                case SerializedPropertyType.Vector3Int:
                    return self.vector3IntValue;
                case SerializedPropertyType.RectInt:
                    return self.rectIntValue;
                case SerializedPropertyType.BoundsInt:
                    return self.boundsIntValue;
                case SerializedPropertyType.ManagedReference:
                    return self.managedReferenceValue;
                case SerializedPropertyType.Hash128:
                    return self.hash128Value;
            }
        }

        public static Gradient GetGradient(this SerializedProperty self)
        {
            System.Reflection.PropertyInfo propertyInfo = typeof(SerializedProperty).GetProperty("gradientValue", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (propertyInfo == null) { return null; }
            else { return propertyInfo.GetValue(self, null) as Gradient; }
        }

#endif
    }
}