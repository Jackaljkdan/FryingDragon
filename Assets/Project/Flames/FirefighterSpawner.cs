using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class FirefighterSpawner : MonoBehaviour
    {
        #region Inspector

        public AssetReferenceComponent<FirefighterInput> firefighterAsset;

        public Transform parent;
        public Transform anchor;

        [RuntimeField]
        public FirefighterInput spawned;

        private void Reset()
        {
            parent = transform.root;
        }

        [ContextMenu("Spawn")]
        private void SpawnInEditMode()
        {
            if (Application.isPlaying)
                Spawn();
        }

        #endregion

        public void Spawn()
        {
            firefighterAsset.InstantiateAsync(anchor.position, anchor.rotation, parent).Completed += handle =>
            {
                spawned = handle.Result;
            };
        }
    }
}