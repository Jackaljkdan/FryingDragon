using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [DisallowMultipleComponent]
    public class Flammable : AbstractInteractable
    {
        #region Inspector

        public float extinguishSeconds = 1f;

        public new ParticleSystem particleSystem;

        [Injected]
        public FirefighterSpawner firefighterSpawner;

        [Injected]
        private SignalBus signalBus;

        private void Reset()
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }

        [ContextMenu("Start fire")]
        private void StartFireInEditMode()
        {
            if (Application.isPlaying)
                StartFire();
        }

        [ContextMenu("Stop fire")]
        private void StopFireInEditMode()
        {
            if (Application.isPlaying)
                StopFire();
        }

        #endregion

        public bool IsOnFire => particleSystem.isPlaying;

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            firefighterSpawner = context.Get<FirefighterSpawner>(this);
            signalBus = context.Get<SignalBus>(this);
        }

        private void Awake()
        {
            Inject();
        }

        protected override void Start()
        {
            base.Start();
            particleSystem.gameObject.SetActive(false);
        }

        public void StartFire()
        {
            if (IsOnFire)
                return;

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();

            signalBus.Invoke(new FireStartSignal(this));
        }

        public void StopFire()
        {
            if (!IsOnFire)
                return;

            particleSystem.Stop();

            signalBus.Invoke(new FireStopSignal(this));
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            if (firefighterSpawner.spawned == null)
                return;

            var firefighterInput = firefighterSpawner.spawned;

            firefighterInput.extinguisherParticleSystem.Play();
            firefighterInput.enabled = false;

            Transform firefighterTransform = firefighterInput.movement.characterControllerTransform;
            firefighterTransform.DORotate(
                Quaternion.LookRotation((transform.position - firefighterTransform.position).WithY(0).normalized).eulerAngles,
                0.5f
            );

            Invoke(nameof(StopExtinguishing), extinguishSeconds);
        }

        private void StopExtinguishing()
        {
            if (firefighterSpawner.spawned != null)
            {
                firefighterSpawner.spawned.extinguisherParticleSystem.Stop();
                firefighterSpawner.spawned.enabled = true;
            }

            StopFire();
        }
    }
}