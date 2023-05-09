using JK.Dev.SceneSetup;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Horror.Dev.Editor
{
    public static class SetupScenes
    {
        [MenuItem("JK/Setup scenes in build settings")]
        private static void Setup()
        {
            var originalSetup = EditorSceneManager.GetSceneManagerSetup();
            EditorSceneManager.SaveScene(SceneManager.GetSceneAt(0));

            EditorUtility.DisplayProgressBar("Loading scenes", "mmmfffppp", 0);

            List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
            List<EditorBuildSettingsScene> scenesLeft = new List<EditorBuildSettingsScene>(scenes);

            float progressRatioForScene = 1f / scenes.Count;

            processNextScene();

            void processNextScene()
            {
                if (scenesLeft.Count > 0)
                {
                    EditorBuildSettingsScene editorScene = scenesLeft[0];
                    float progress = ((float)(scenes.Count - scenesLeft.Count)) / scenes.Count;

                    //string sceneName = Path.GetFileNameWithoutExtension(editorScene.path);
                    //Debug.Log(sceneName);

                    EditorSceneManager.OpenScene(editorScene.path, OpenSceneMode.Single);

                    Scene scene = SceneManager.GetSceneAt(0);
                    SetupScene(scene, progress, progressRatioForScene, onComplete: () =>
                    {
                        scenesLeft.RemoveAt(0);
                        processNextScene();
                    });
                }
                else
                {
                    EditorUtility.ClearProgressBar();
                    EditorSceneManager.OpenScene(originalSetup[0].path, OpenSceneMode.Single);
                }
            }
        }

        [MenuItem("JK/Setup current scene")]
        private static void SetupCurrentScene()
        {
            SetupScene(SceneManager.GetSceneAt(0), progress: 0, ratioForScene: 1, clearProgressBar: true);
        }

        private static void SetupScene(Scene scene, float progress, float ratioForScene, bool clearProgressBar = false, UnityAction onComplete = null)
        {
            EditorUtility.DisplayProgressBar($"Setting up {scene.name}", "mmmfffppp", progress);

            GameObject[] roots = scene.GetRootGameObjects();
            List<IEditorSceneSetup> setups = roots.SelectMany(r => r.GetComponentsInChildren<IEditorSceneSetup>(includeInactive: true)).ToList();

            int tasksCount = setups.Count + 1;
            int tasksDone = 0;

            foreach (var sceneSetup in setups)
            {
                EditorUtility.DisplayProgressBar(
                    sceneSetup.GetEditorSceneSetupTitle() ?? $"Setting up {scene.name}",
                    sceneSetup.GetEditorSceneSetupDescription() ?? "mmmfffppp",
                    progress + (ratioForScene * tasksDone / tasksCount)
                );

                sceneSetup.EditorSceneSetup();

                tasksDone++;
            }

            EditorUtility.DisplayProgressBar($"Saving {scene.name}", "mmmfffppp", progress + (ratioForScene * tasksDone / tasksCount));
            EditorSceneManager.SaveScene(scene);

            if (clearProgressBar)
                EditorUtility.ClearProgressBar();

            onComplete?.Invoke();
        }
    }
}