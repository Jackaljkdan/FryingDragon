using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class TypeCacheUtils
    {
        public static IList<MethodInfo> GetMethodsWithAttribute(Type attrType)
        {
#if UNITY_EDITOR
            return TypeCache.GetMethodsWithAttribute(attrType);
#else
            return null;
#endif
        }

        public static IList<MethodInfo> GetMethodsWithAttribute<T>() where T : Attribute
        {
#if UNITY_EDITOR
            return TypeCache.GetMethodsWithAttribute<T>();
#else
            return null;
#endif
        }
    }
}