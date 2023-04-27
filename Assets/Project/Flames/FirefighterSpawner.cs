using JK.Injection;
using JK.Utils;
using JK.Utils.Addressables;
using Project.Dragon;
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

        [Injected]
        public DragonStress dragonStress;

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

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            dragonStress = context.Get<DragonStress>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void OnEnable()
        {
            dragonStress.onFrenzy.AddListener(OnDragonFrenzy);
        }

        private void OnDisable()
        {
            dragonStress.onFrenzy.RemoveListener(OnDragonFrenzy);
        }

        private void OnDragonFrenzy()
        {
            Spawn();
        }

        public void Spawn()
        {
            firefighterAsset.InstantiateAsync(anchor.position, anchor.rotation, parent).Completed += handle =>
            {
                spawned = handle.Result;
            };
        }
    }
}