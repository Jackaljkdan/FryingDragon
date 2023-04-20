using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Editor.Shortcuts
{
    public static class HighlightInHierarchyShortcut
    {
        private static Type hierarchyWindowType;

        [MenuItem("JK/Highlight Selected Object in Hierarchy %h")]
        private static void HighlightSelectedObjectInHierarchy()
        {
            if (Selection.activeObject != null)
            {
                if (hierarchyWindowType == null)
                {
                    // https://stackoverflow.com/questions/54431635/is-there-a-keyboard-shortcut-to-maximize-the-game-window-in-unity-in-play-mode/62594378#62594378
                    Assembly assembly = typeof(EditorWindow).Assembly;
                    hierarchyWindowType = assembly.GetType("UnityEditor.SceneHierarchyWindow");
                }

                EditorWindow.FocusWindowIfItsOpen(hierarchyWindowType);
                EditorGUIUtility.PingObject(Selection.activeObject);
            }
        }
    }
}