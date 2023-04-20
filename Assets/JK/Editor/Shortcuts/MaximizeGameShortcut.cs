using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.Shortcuts
{
    /// <summary>
    /// https://stackoverflow.com/questions/54431635/is-there-a-keyboard-shortcut-to-maximize-the-game-window-in-unity-in-play-mode/60288734#60288734
    /// </summary>
    [InitializeOnLoad]
    public class MaximizeGameShortcut
    {
        private static float keysDownIntervalSeconds = 1;
        private static float lastKeysDownTime;

        static MaximizeGameShortcut()
        {
            EditorApplication.update += Update;
        }

        static void Update()
        {
            if (EditorApplication.isPlaying && ShouldToggleMaximize())
            {
                EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;

                if (EditorWindow.focusedWindow.maximized)
                    Cursor.lockState = CursorLockMode.Confined;
                else
                    Cursor.lockState = CursorLockMode.None;
            }
        }

        private static Type gameViewType;

        private static bool ShouldToggleMaximize()
        {
            if (!UnityEngine.Input.GetKey(KeyCode.Space) || !UnityEngine.Input.GetKey(KeyCode.LeftShift))
            {
                lastKeysDownTime = -keysDownIntervalSeconds;
                return false;
            }

            if (Time.time - lastKeysDownTime < keysDownIntervalSeconds)
                return false;

            lastKeysDownTime = Time.time;

            if (gameViewType == null)
            {
                Assembly assembly = typeof(EditorWindow).Assembly;
                gameViewType = assembly.GetType("UnityEditor.GameView");
            }

            return EditorWindow.focusedWindow.GetType() == gameViewType;
        }
    }
}