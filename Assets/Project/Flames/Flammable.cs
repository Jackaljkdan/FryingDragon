using DG.Tweening;
using JK.Injection;
using JK.Interaction;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public ParticleSystem extinguisherParticleSystem;

        [Injected]
        public FirefighterInput firefighterInput;

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
            extinguisherParticleSystem = context.Get<ParticleSystem>(this, "firefighter.particle");
            firefighterInput = context.Get<FirefighterInput>(this);
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
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
        }

        public void StopFire()
        {
            particleSystem.Stop();
        }

        protected override void InteractProtected(RaycastHit hit)
        {
            extinguisherParticleSystem.Play();
            firefighterInput.enabled = false;

            Transform firefighterTransform = firefighterInput.firefighterMovement.characterControllerTransform;
            firefighterTransform.DORotate(
                Quaternion.LookRotation((transform.position - firefighterTransform.position).WithY(0).normalized).eulerAngles,
                0.5f
            );

            Invoke(nameof(StopExtinguishing), extinguishSeconds);
        }

        private void StopExtinguishing()
        {
            StopFire();
            extinguisherParticleSystem.Stop();
            firefighterInput.enabled = true;
        }
    }
}