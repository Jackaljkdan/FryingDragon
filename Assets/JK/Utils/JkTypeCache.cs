using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class JkTypeCache
    {
        private class CachedInfo
        {
            public List<MethodInfo> methods;
            public Dictionary<Type, List<MethodInfo>> methodsWithAttribute;
        }

        private static Dictionary<Type, CachedInfo> _cacheDict;

        private static Dictionary<Type, CachedInfo> CacheDict
        {
            get
            {
                if (_cacheDict == null)
                    _cacheDict = new Dictionary<Type, CachedInfo>();

                return _cacheDict;
            }
        }

        private static CachedInfo GetInfo(Type type)
        {
            var cache = CacheDict;

            try
            {
                return cache[type];
            }
            catch (KeyNotFoundException)
            {
                var info = new CachedInfo();
                cache[type] = info;
                return info;
            }
        }

        public static IReadOnlyList<MethodInfo> GetMethods(Type type)
        {
            var info = GetInfo(type);

            if (info.methods == null)
                info.methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).ToList();

            return info.methods;
        }

        public static IReadOnlyList<MethodInfo> GetMethods<T>()
        {
            return GetMethods(typeof(T));
        }

        public static IReadOnlyList<MethodInfo> GetMethodsWithAttribute(Type type, Type attributeType)
        {
            var info = GetInfo(type);

            if (info.methodsWithAttribute == null)
                info.methodsWithAttribute = new Dictionary<Type, List<MethodInfo>>();

            try
            {
                return info.methodsWithAttribute[attributeType];
            }
            catch (KeyNotFoundException)
            {
                List<MethodInfo> methodsWithAttribute = new List<MethodInfo>();

                var collection = TypeCacheUtils.GetMethodsWithAttribute(attributeType);

                foreach (var methodInfo in collection)
                    if (methodInfo.DeclaringType == type || type.IsSubclassOf(methodInfo.DeclaringType))
                        methodsWithAttribute.Add(methodInfo);

                info.methodsWithAttribute[attributeType] = methodsWithAttribute;

                return methodsWithAttribute;
            }
        }

        public static IReadOnlyList<MethodInfo> GetMethodsWithAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return GetMethodsWithAttribute(type, typeof(TAttribute));
        }

        public static IReadOnlyList<MethodInfo> GetMethodsWithAttribute<TObject, TAttribute>() where TAttribute : Attribute
        {
            return GetMethodsWithAttribute(typeof(TObject), typeof(TAttribute));
        }
    }
}