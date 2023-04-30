using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class GameObjectUtils
    {
        public const string EditorOnlyTag = "EditorOnly";

        public static void DestroyEditorFriendly(this GameObject self)
        {
            if (PlatformUtils.IsEditor && !Application.isPlaying)
                GameObject.DestroyImmediate(self);
            else
                GameObject.Destroy(self);
        }

        public static void MarkEditorOnly(this GameObject self)
        {
            self.tag = EditorOnlyTag;
            UndoUtils.SetDirty(self);
        }

        public static bool IsEditorOnly(this GameObject self)
        {
            return self.CompareTag(EditorOnlyTag);
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            if (!self.TryGetComponent(out T component))
                component = self.AddComponent<T>();

            return component;
        }

        public static T GetComponentSafely<T>(this GameObject self)
        {
            if (self == null)
                return default;

            return self.GetComponent<T>();
        }

        public static bool TryGetComponentInChildren<T>(this GameObject self, out T component)
        {
            component = self.GetComponentInChildren<T>();
            return component != null;
        }

        public static void CopyStaticFlagsInEditorTo(this GameObject self, GameObject other)
        {
#if UNITY_EDITOR
            StaticEditorFlags myFlags = GameObjectUtility.GetStaticEditorFlags(self);
            GameObjectUtility.SetStaticEditorFlags(other, myFlags);
#endif
        }
    }
}