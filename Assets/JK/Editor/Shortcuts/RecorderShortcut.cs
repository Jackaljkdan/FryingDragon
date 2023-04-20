using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.Shortcuts
{
    [InitializeOnLoad]
    public static class RecorderShortcut
    {
        private static Type RecorderWindowType;
        private static MethodInfo ShowRecorderWindowMethod;
        private static MethodInfo QuickRecordingMethod;

        private static EditorKey WindowKey = new EditorKey(KeyCode.F12);

        static RecorderShortcut()
        {
            EditorApplication.update += Update;
        }

        [MenuItem("JK/Show Recorder Window _F12")]
        private static void ShowRecorderWindow()
        {
            SetupIfNeeded();
            ShowRecorderWindowMethod.Invoke(null, null);
        }

        [MenuItem("JK/Start Recording #F12")]
        private static void StartRecording()
        {
            SetupIfNeeded();
            QuickRecordingMethod.Invoke(null, null);
        }

        // this is for playmode
        static void Update()
        {
            if (!WindowKey.IsDown())
                return;

            SetupIfNeeded();

            if (!InputUtils.GetAnyShift())
                ShowRecorderWindowMethod.Invoke(null, null);
            else
                QuickRecordingMethod.Invoke(null, null);
        }

        static void SetupIfNeeded()
        {
            if (RecorderWindowType == null)
            {
                Assembly asm = Assembly.Load("Unity.Recorder.Editor, Version = 0.0.0.0, Culture = neutral, PublicKeyToken = null");
                RecorderWindowType = asm.GetType("UnityEditor.Recorder.RecorderWindow");
            }

            if (ShowRecorderWindowMethod == null)
                ShowRecorderWindowMethod = RecorderWindowType.GetMethod("ShowRecorderWindow", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (QuickRecordingMethod == null)
                QuickRecordingMethod = RecorderWindowType.GetMethod("QuickRecording", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        }
    }
}