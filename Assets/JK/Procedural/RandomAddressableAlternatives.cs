using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RandomAddressableAlternatives : MonoBehaviour
    {
        #region Inspector

        public bool chooseOnStart = true;

        public bool canChooseNone = true;

        public Transform instancesParent;

        public List<AssetReferenceGameObject> assets;

        [RuntimeField]
        public List<GameObject> instances;

        [RuntimeField]
        public int currentChoiceIndex;

        [RuntimeField]
        public bool isSpawning;

        private void Reset()
        {
            instancesParent = transform;
        }

        [ContextMenu("Spawn First")]
        public void SpawnFirstInEditMode()
        {
            if (assets == null || assets.Count == 0)
                return;

            Spawn(0, go => go.MarkEditorOnly());

            UndoUtils.SetDirty(this);
        }

        [ContextMenu("Spawn All")]
        public void SpawnAllInEditMode()
        {
            if (assets == null || assets.Count == 0)
                return;

            for (int i = 0; i < assets.Count; i++)
                Spawn(i, callback: go => go.MarkEditorOnly());

            UndoUtils.SetDirty(this);
        }

        [ContextMenu("Destroy All Spawned")]
        public void DestroyAllSpawnedInEditMode()
        {
            transform.DestroyChildren();
            UndoUtils.SetDirty(this);
        }

        [ContextMenu("Choose Again")]
        public void ChooseAgainInEditMode()
        {
            if (Application.isPlaying)
                Choose();
        }

        #endregion

        private void Start()
        {
            if (PlatformUtils.IsEditor)
                DestroyAllSpawnedInEditMode();

            currentChoiceIndex = -1;
            isSpawning = false;
            InitIfNeeded();

            if (chooseOnStart)
                Choose();
        }

        public void InitIfNeeded()
        {
            if (instances != null && instances.Count == assets.Count)
                return;

            Init();
        }

        public void Init()
        {
            instances = new List<GameObject>(assets.Count);

            for (int i = 0; i < assets.Count; i++)
                instances.Add(null);
        }

        public void Choose()
        {
            Choose(callback: null);
        }

        public void Choose(UnityAction<GameObject> callback)
        {
            if (isSpawning)
                return;

            int randomIndex = UnityEngine.Random.Range(canChooseNone ? -1 : 0, assets.Count);
            Choose(randomIndex, callback);
        }

        private void Choose(int index, UnityAction<GameObject> callback)
        {
            if (currentChoiceIndex >= 0)
                instances[currentChoiceIndex].SetActive(false);

            currentChoiceIndex = index;

            if (index >= 0)
                Spawn(index, callback);
        }

        private void Spawn(int index, UnityAction<GameObject> callback)
        {
            GameObject instance = instances.AtCatched(index);
            isSpawning = true;

            if (instance == null)
            {
                if (!PlatformUtils.IsEditor || Application.isPlaying)
                {
                    assets[index].InstantiateAsync(instancesParent, instantiateInWorldSpace: false).Completed += handle =>
                    {
                        if (!PlatformUtils.IsEditor || Application.isPlaying)
                            instances[index] = handle.Result;

                        completeSpawn(handle.Result);
                    };
                }
                else
                {
                    GameObject prefabInstance = AssetReferenceUtils.InstantiateAsPrefab(assets[index], instancesParent);
                    completeSpawn(prefabInstance);
                }
            }
            else
            {
                completeSpawn(instance);
            }

            void completeSpawn(GameObject go)
            {
                isSpawning = false;
                go.SetActive(true);
                callback?.Invoke(go);
            }
        }
    }
}