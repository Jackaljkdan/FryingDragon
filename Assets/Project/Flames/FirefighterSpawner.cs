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

        public AssetReferenceComponent<FirefighterExit> firefighterAsset;

        [RuntimeField]
        public FirefighterExit spawned;

        [Injected]
        public Transform anchor;

        [Injected]
        public DragonStress dragonStress;

        [Injected]
        private SignalBus signalBus;

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
            anchor = context.Get<Transform>(this, "firefighter.spawn");
            dragonStress = context.Get<DragonStress>(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        private void OnEnable()
        {
            signalBus.AddListener<FireStartSignal>(OnFireStart);
        }

        private void OnDisable()
        {
            signalBus.RemoveListener<FireStartSignal>(OnFireStart);

            if (spawned != null)
                spawned.onExit.RemoveListener(OnSpawnedExit);
        }

        private void OnFireStart(FireStartSignal arg)
        {
            if (spawned != null)
                return;

            if (!dragonStress.isInFrenzy)
                return;

            if (IsInvoking(nameof(Spawn)))
                return;

            Invoke(nameof(Spawn), 1);
        }

        public void Spawn()
        {
            firefighterAsset.InstantiateAsync(anchor.position, anchor.rotation, transform.root).Completed += handle =>
            {
                spawned = handle.Result;
                spawned.onExit.AddListener(OnSpawnedExit);

                signalBus.Invoke(new FirefighterSpawnSignal() { firefighter = spawned });
            };
        }

        private void OnSpawnedExit()
        {
            firefighterAsset.ReleaseInstance(spawned.gameObject);
            spawned = null;
        }
    }
}