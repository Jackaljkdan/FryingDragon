using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class AngryAnimator : MonoBehaviour
    {
        #region Inspector

        public Animator animator;
        public ParticleSystem fireParticles;

        public new Transform camera;

        public Transform cameraStartAnchor;
        public Transform cameraEndAnchor;
        public Transform cameraLookTarget;

        public AnimationCurve speed;

        [DebugField]
        public float elapsed = 0;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        private void Update()
        {
            float deltaTime = Time.deltaTime * speed.Evaluate(elapsed);
            elapsed += deltaTime;
            //animator.speed = speed.Evaluate(elapsed);
            animator.Update(deltaTime);

            camera.position = Vector3.Lerp(cameraStartAnchor.position, cameraEndAnchor.position, elapsed / 1.7f);
            camera.rotation = Quaternion.LookRotation((cameraLookTarget.position - camera.position).normalized);
        }

        public void OnAngryFire()
        {
            fireParticles.gameObject.SetActive(true);
            fireParticles.Play();
        }

        public void OnAngryFireStop()
        {
            fireParticles.Stop();
        }

    }
}