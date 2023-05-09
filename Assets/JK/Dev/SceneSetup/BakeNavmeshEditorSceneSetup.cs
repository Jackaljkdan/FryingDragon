using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace JK.Dev.SceneSetup
{
    [DisallowMultipleComponent]
    public class BakeNavmeshEditorSceneSetup : MonoBehaviour, IEditorSceneSetup
    {
        #region Inspector

        public bool useRoot = true;

        [Conditional(nameof(useRoot), inverse: true)]
        public GameObject surfaceObject;

        private void Reset()
        {
            if (transform.root.TryGetComponentInChildren(out NavMeshSurface navMeshSurface))
                surfaceObject = navMeshSurface.gameObject;
            else
                surfaceObject = transform.root.gameObject;
        }

        #endregion

        public void EditorSceneSetup()
        {
            var scene = gameObject.scene;

            if (useRoot)
                surfaceObject = transform.root.gameObject;

            if (!surfaceObject.TryGetComponentInChildren(out NavMeshSurface surface))
                surface = surfaceObject.AddComponent<NavMeshSurface>();

            if (surface.navMeshData != null)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(surface.navMeshData));

            surface.BuildNavMesh();
            AssetDatabase.CreateAsset(surface.navMeshData, $"{Path.GetDirectoryName(scene.path)}/{scene.name}/NavMesh-{scene.name}.asset");
        }

        public string GetEditorSceneSetupTitle()
        {
            return $"Bake {gameObject.scene.name} navmesh";
        }
    }
}