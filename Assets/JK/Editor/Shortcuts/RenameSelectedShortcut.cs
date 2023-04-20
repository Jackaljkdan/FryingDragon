using System;
using UnityEditor;
using UnityEngine;

namespace JK.Editor.Shortcuts
{
    /// <summary>
    /// https://answers.unity.com/questions/1758798/is-there-anyway-to-batch-renaming-via-editor-not-o.html?childToView=1914254#comment-1914254
    /// </summary>
    public class RenameSelectedShortcut : EditorWindow
    {
        private string inputName;
        private int inputIndex;

        [MenuItem("JK/Rename Selected #F2")]
        public static void ShowWindow()
        {
            RenameSelectedShortcut window = GetWindow<RenameSelectedShortcut>();
            Vector2Int size = new Vector2Int(250, 100);
            window.minSize = size;
            window.maxSize = size;
            window.titleContent = new GUIContent("Rename Selected");
        }

        private GameObject[] GetSortedSelectedGameObjects()
        {
            var unsortedGameObjects = Selection.gameObjects;
            var sortedGameObjects = new GameObject[unsortedGameObjects.Length];
            for (var i = 0; i < unsortedGameObjects.Length; i++)
                sortedGameObjects[i] = unsortedGameObjects[i];

            Array.Sort(sortedGameObjects, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

            return sortedGameObjects;
        }

        private void OnGUI()
        {
            inputName = EditorGUILayout.TextField("Name", inputName);
            inputIndex = EditorGUILayout.IntField("Start Index", inputIndex);

            GUI.enabled = false;
            EditorGUILayout.IntField("Count (hover to refresh)", Selection.count);
            GUI.enabled = true;

            if (GUILayout.Button("Rename"))
            {
                GameObject[] gameObjects = GetSortedSelectedGameObjects();
                Undo.RecordObjects(gameObjects, "Rename Selected");

                int i = inputIndex;

                foreach (var go in gameObjects)
                    go.name = $"{inputName} ({i++})";
            }
        }
    }
}