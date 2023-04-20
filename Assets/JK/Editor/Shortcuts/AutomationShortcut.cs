using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace JK.Editor.Shortcuts
{
    public static class AutomationShortcut
    {
        [MenuItem("JK/Run automated tasks #r")]
        private static void RunAutomatedTasks()
        {
            var stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
            {
                RunAutomatedTasksOnGameObject(stage.prefabContentsRoot);
                return;
            }

            for (int i = 0; i < SceneManager.sceneCount; i++)
                RunAutomatedTasksOnScene(SceneManager.GetSceneAt(i));
        }

        private static void RunAutomatedTasksOnScene(Scene scene)
        {
            foreach (var rootObject in scene.GetRootGameObjects())
                RunAutomatedTasksOnGameObject(rootObject);
        }

        private static void RunAutomatedTasksOnGameObject(GameObject gameObject)
        {
            foreach (var monoBehaviour in gameObject.GetComponentsInChildren<MonoBehaviour>())
                RunAutomatedTasksOnMonoBehaviour(monoBehaviour);
        }

        private static void RunAutomatedTasksOnMonoBehaviour(MonoBehaviour monoBehaviour)
        {
            Type type = monoBehaviour.GetType();

            foreach (MethodInfo method in JkTypeCache.GetMethodsWithAttribute<AutomatedTaskAttribute>(type))
                method.Invoke(monoBehaviour, null);
        }
    }
}