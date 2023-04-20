using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class ComponentUtils
    {
        public static T GetOrAddComponent<T>(this Component self) where T : Component
        {
            return self.gameObject.GetOrAddComponent<T>();
        }

        public static T GetComponenetSafely<T>(this Component self)
        {
            if (self == null)
                return default;

            return self.GetComponent<T>();
        }

        public static GameObject GetGameObjectSafely(this Component self)
        {
            if (self == null)
                return null;
            else
                return self.gameObject;
        }

        public static bool TryGetComponentInParent<T>(this Component self, out T component)
        {
            component = self.GetComponentInParent<T>();
            return component != null;
        }

        public static bool TryGetComponentInChildren<T>(this Component self, out T component)
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
        }

        public static T GetComponentInParentOrChildren<T>(this Component self)
        {
            if (self.TryGetComponentInParent(out T component))
                return component;
            else
                return self.GetComponentInChildren<T>();
        }

        public static T GetComponentInChildrenOrParent<T>(this Component self)
        {
            if (self.TryGetComponentInChildren(out T component))
                return component;
            else
                return self.GetComponentInParent<T>();
        }

        public static bool TrySetEnabled<T>(this Component self, bool enabled)
        {
            if (self.TryGetComponent(out T component) && (component is Behaviour behaviour))
            {
                behaviour.enabled = enabled;
                return true;
            }

            return false;
        }

        public static string GetHierarchyName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;

            string hierarchyName = self.name;

            Transform parent = self.transform.parent;

            while (parent != null)
            {
                hierarchyName = $"{parent.name} > {hierarchyName}";
                parent = parent.parent;
            }

            return $"({hierarchyName})";
        }

        public static string GetClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return self.GetType().Name;
        }

        public static string GetHierarchyDotClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return $"{self.GetHierarchyName()}.{self.GetClassName()}";
        }

        public static string GetGameObjectDotClassName(this Component self)
        {
            if (self == null)
                return UnityObjectUtils.NullName;
            else
                return $"{self.GetName()}.{self.GetClassName()}";
        }

        public static string Contextualize(this Component self, string message, bool includeHierarchy = false, bool includeClass = false)
        {
            string context;

            if (includeHierarchy)
            {
                if (includeClass)
                    context = self.GetHierarchyDotClassName();
                else
                    context = self.GetHierarchyName();
            }
            else
            {
                if (includeClass)
                    context = self.GetGameObjectDotClassName();
                else
                    context = self.GetName();
            }

            return $"[{context}] {message}";
        }
    }
}