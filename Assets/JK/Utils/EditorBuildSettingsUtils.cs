using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class EditorBuildSettingsUtils
    {
        public static bool ContainsScene(string path)
        {
#if UNITY_EDITOR
            foreach (var s in EditorBuildSettings.scenes)
                if (s.path == path)
                    return true;
#endif

            return false;
        }

        public static void AddScene(string path)
        {
#if UNITY_EDITOR
            if (ContainsScene(path))
                return;

            EditorBuildSettingsScene[] array = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + 1];
            EditorBuildSettings.scenes.CopyTo(array, 0);

            array[EditorBuildSettings.scenes.Length] = new EditorBuildSettingsScene(path, enabled: true);

            EditorBuildSettings.scenes = array;
#endif
        }

        public static void AddScenes(IList<string> paths)
        {
#if UNITY_EDITOR
            int nonContained = 0;

            foreach (var path in paths)
                if (!ContainsScene(path))
                    nonContained++;

            EditorBuildSettingsScene[] array = new EditorBuildSettingsScene[EditorBuildSettings.scenes.Length + nonContained];
            EditorBuildSettings.scenes.CopyTo(array, 0);

            int i = EditorBuildSettings.scenes.Length;

            foreach (var path in paths)
                if (!ContainsScene(path))
                    array[i++] = new EditorBuildSettingsScene(path, enabled: true);

            EditorBuildSettings.scenes = array;
#endif
        }
    }
}