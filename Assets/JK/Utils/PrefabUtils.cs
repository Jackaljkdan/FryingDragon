using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace JK.Utils
{
    public static class PrefabUtils
    {
        public static UnityEngine.Object InstantiatePrefab(UnityEngine.Object prefab, Transform parent)
        {
#if UNITY_EDITOR
            return PrefabUtility.InstantiatePrefab(prefab, parent);
#else
            return null;
#endif
        }

        /// <summary>
        /// N.B. non è deprecato ma quello che restituisce è inutile o non l'ho capito.
        /// Se vuoi sapere se un oggetto è il prefab aperto correntemente chiama
        /// <see cref="IsEditedPrefab"/>.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        [Obsolete]
        public static UnityEngine.Object GetPrefab(GameObject gameObject)
        {
#if UNITY_EDITOR
            return PrefabUtility.GetPrefabInstanceHandle(gameObject);
#else
            return null;
#endif
        }

        // questi non funzionano, forse non ho capito la doc

        //        public static bool TryGetPrefab(GameObject gameObject, out UnityEngine.Object prefab)
        //        {
        //            prefab = GetPrefab(gameObject);
        //            return prefab != null;
        //        }

        //        public static T GetPrefab<T>(T component) where T : Component
        //        {
        //#if UNITY_EDITOR
        //            var p = PrefabUtility.GetPrefabInstanceHandle(component);
        //            return p as T;
        //#else
        //            return null;
        //#endif
        //        }

        //        public static bool TryGetPrefab<T>(T component, out T prefab) where T : Component
        //        {
        //            prefab = GetPrefab(component);
        //            return prefab != null;
        //        }

        public static T GetBasePrefab<T>(T component) where T : Component
        {
#if UNITY_EDITOR
            return PrefabUtility.GetCorrespondingObjectFromSource(component);
#else
            return null;
#endif
        }

        public static bool TryGetBasePrefab<T>(T component, out T basePrefab) where T : Component
        {
            basePrefab = GetBasePrefab(component);
            return basePrefab != null;
        }

        public static GameObject GetBasePrefab(GameObject gameObject)
        {
#if UNITY_EDITOR
            return PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
#else
            return null;
#endif
        }

        public static bool TryGetBasePrefab(GameObject gameObject, out GameObject basePrefab)
        {
            basePrefab = GetBasePrefab(gameObject);
            return basePrefab != null;
        }

        /// <summary>
        /// N.B. non è deprecato, in realtà restituisce il path del prefab padre e non ho idea di come ottenere il path del prefab passato.
        /// Se vuoi il path del prefab aperto correntememnte chiama <see cref="GetCurrentPrefabPath"/>.
        /// </summary>
        [Obsolete]
        public static string GetPrefabPath(UnityEngine.Object instanceComponentOrGameObject)
        {
#if UNITY_EDITOR
            return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instanceComponentOrGameObject);
#else
            return null;
#endif
        }

        // questi in realtà restituiscono cose che riguardano il prefab padre e non il prefab attuale
        //        public static bool TryGetPrefabPath(UnityEngine.Object instanceComponentOrGameObject, out string path)
        //        {
        //            path = GetPrefabPath(instanceComponentOrGameObject);
        //            return path != null;
        //        }

        //        public static string GetPrefabGuid(UnityEngine.Object instanceComponentOrGameObject)
        //        {
        //            if (TryGetPrefabPath(instanceComponentOrGameObject, out string path))
        //                return AssetDatabaseUtils.GUIDFromAssetPath(path);

        //            return null;
        //        }

        //        public static bool TryGetPrefabGuid(UnityEngine.Object instanceComponentOrGameObject, out string guid)
        //        {
        //            guid = GetPrefabGuid(instanceComponentOrGameObject);
        //            return guid != null;
        //        }

        public static bool IsPrefabMode()
        {
#if UNITY_EDITOR
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
#else
            return false;
#endif
        }

        public static bool IsEditedPrefab(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            var stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
            {
                if (obj is GameObject go)
                    return stage.prefabContentsRoot == go;
                else if (obj is Component component)
                    return stage.prefabContentsRoot == component.gameObject;
            }
#endif

            return false;
        }

        public static string GetCurrentPrefabPath()
        {
#if UNITY_EDITOR
            var stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
                return stage.assetPath;
#endif

            return null;
        }

        public static string GetCurrentPrefabGuid()
        {
#if UNITY_EDITOR
            var stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
                return AssetDatabaseUtils.GUIDFromAssetPath(stage.assetPath);
#endif

            return null;
        }

        public static void ApplyAutomatedObjectOverride(UnityEngine.Object instanceComponentOrGameObject, string assetPath)
        {
#if UNITY_EDITOR
            PrefabUtility.ApplyObjectOverride(instanceComponentOrGameObject, assetPath, InteractionMode.AutomatedAction);
#endif
        }

        public static void ApplyAutomatedPropertyOverride(UnityEngine.Object instanceComponentOrGameObject, string propertyPath, string assetPath)
        {
#if UNITY_EDITOR
            SerializedObject serializedObject = new SerializedObject(instanceComponentOrGameObject);
            SerializedProperty property = serializedObject.FindProperty(propertyPath);
            PrefabUtility.ApplyPropertyOverride(property, assetPath, InteractionMode.AutomatedAction);
#endif
        }

        public static void ApplyAutomatedAddedGameObject(GameObject gameObjects, string assetPath)
        {
#if UNITY_EDITOR
            PrefabUtility.ApplyAddedGameObject(gameObjects, assetPath, InteractionMode.AutomatedAction);
#endif
        }

        public static Exception ApplyAutomatedAddedGameObjectCaught(GameObject gameObjects, string assetPath)
        {
            try
            {
                ApplyAutomatedAddedGameObject(gameObjects, assetPath);
                return null;
            }
            catch (ArgumentException ex)
            {
                return ex;
            }
        }

        public static void RecordPrefabInstancePropertyModifications(UnityEngine.Object targetObject)
        {
#if UNITY_EDITOR
            PrefabUtility.RecordPrefabInstancePropertyModifications(targetObject);
#endif
        }

        public static void SavePrefabAsset(GameObject asset, out bool savedSuccessfully)
        {
#if UNITY_EDITOR
            PrefabUtility.SavePrefabAsset(asset, out savedSuccessfully);
#else
            savedSuccessfully = false;
#endif
        }

        public static void SavePrefabAsset(GameObject asset)
        {
            SavePrefabAsset(asset, out _);
        }

        public static void SaveCurrentPrefab()
        {
#if UNITY_EDITOR
            var stage = PrefabStageUtility.GetCurrentPrefabStage();

            if (stage != null)
                AssetDatabase.SaveAssetIfDirty(AssetDatabase.GUIDFromAssetPath(stage.assetPath));
#endif
        }

        public static void ApplyAutomatedAddedGameObjectOrOverrides(GameObject gameObject, string assetPath)
        {
            try
            {
                ApplyAutomatedAddedGameObject(gameObject, assetPath);
            }
            catch (ArgumentException)
            {
                ApplyAutomatedObjectOverride(gameObject, assetPath);

                foreach (var component in gameObject.GetComponents<Component>())
                    ApplyAutomatedObjectOverride(component, assetPath);

                foreach (Transform child in gameObject.transform)
                    ApplyAutomatedAddedGameObjectOrOverrides(child.gameObject, assetPath);
            }
        }

        public static void RevertEverything(GameObject gameObject, List<UnityEngine.Object> excluded = null)
        {
#if UNITY_EDITOR
            HashSet<GameObject> reverted = new HashSet<GameObject>();

            if (excluded == null || !excluded.Contains(gameObject))
                PrefabUtility.RevertObjectOverride(gameObject, InteractionMode.AutomatedAction);

            if (excluded == null || !excluded.Contains(gameObject.transform))
                PrefabUtility.RevertObjectOverride(gameObject.transform, InteractionMode.AutomatedAction);

            reverted.Add(gameObject);
            UndoUtils.SetDirty(gameObject);

            foreach (var component in gameObject.GetComponentsInChildren<Component>(includeInactive: true))
            {
                if (excluded == null || !excluded.Contains(component))
                {
                    PrefabUtility.RevertObjectOverride(component, InteractionMode.AutomatedAction);
                    UndoUtils.SetDirty(component);
                }

                if (!reverted.Contains(component.gameObject))
                {
                    if (excluded == null || !excluded.Contains(component.gameObject))
                        PrefabUtility.RevertObjectOverride(component.gameObject, InteractionMode.AutomatedAction);

                    if (excluded == null || !excluded.Contains(component.transform))
                        PrefabUtility.RevertObjectOverride(component.transform, InteractionMode.AutomatedAction);

                    reverted.Add(component.gameObject);
                    UndoUtils.SetDirty(component.gameObject);
                }
            }
#endif
        }
    }
}