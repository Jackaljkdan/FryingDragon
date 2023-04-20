using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class HandlesUtils
    {
        public static void Label(Vector3 position, string text, TextAnchor alignment = TextAnchor.MiddleCenter, Color? color = null)
        {
#if UNITY_EDITOR
            var style = new GUIStyle();
            style.alignment = alignment;
            style.normal.textColor = color ?? Color.white;
            UnityEditor.Handles.Label(position, text, style);
#endif
        }

        public static void DrawWireDisc(Vector3 position, Vector3 normal, float radius)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.DrawWireDisc(position, normal, radius);
#endif
        }

        public static void DrawWireDisc(Vector3 position, Vector3 normal, float radius, float thickness)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.DrawWireDisc(position, normal, radius, thickness);
#endif
        }

        public static void WithRestoredColor(UnityAction action)
        {
#if UNITY_EDITOR
            Color prevColor = UnityEditor.Handles.color;
#endif

            action();

#if UNITY_EDITOR
            UnityEditor.Handles.color = prevColor;
#endif
        }

        public static void SetColor(Color color)
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = color;
#endif
        }
    }
}