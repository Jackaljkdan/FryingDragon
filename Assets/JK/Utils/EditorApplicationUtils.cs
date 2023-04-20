using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class EditorApplicationUtils
    {
        public static void DelayCall(UnityAction action)
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () => action();
#endif
        }

        public static void AddUpdateListenerOce(UnityAction action)
        {
#if UNITY_EDITOR
            EditorApplication.update += wrapper;

            void wrapper()
            {
                EditorApplication.update -= wrapper;
                action();
            }
#endif
        }

        public static object AddUpdateListener(UnityAction action)
        {
#if UNITY_EDITOR
            EditorApplication.CallbackFunction wrapper = () => action();
            EditorApplication.update += wrapper;
            return wrapper;
#else
            return null;
#endif
        }

        public static void RemoveUpdateListener(object listener)
        {
#if UNITY_EDITOR
            if (listener is EditorApplication.CallbackFunction wrapper)
                EditorApplication.update -= wrapper;
#endif
        }
    }
}