using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class EditorGUIUtils
    {
        public static string GetSystemCopyBuffer()
        {
#if UNITY_EDITOR
            return EditorGUIUtility.systemCopyBuffer;
#else
            return null;
#endif
        }

        public static void SetSystemCopyBuffer(string buffer)
        {
#if UNITY_EDITOR
            EditorGUIUtility.systemCopyBuffer = buffer;
#endif
        }

        public static void PingObject(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            EditorGUIUtility.PingObject(obj);
#endif
        }
    }
}