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
    public static class SerializedPropertyTypeUtils
    {
#if UNITY_EDITOR
        public static bool IsUnityObjectType(this SerializedPropertyType self)
        {
            switch (self)
            {
                case SerializedPropertyType.ExposedReference:
                case SerializedPropertyType.ObjectReference:
                    return true;
                default:
                    return false;
            }
        }
#endif
    }
}