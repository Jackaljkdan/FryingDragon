using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Dragon
{
    [DisallowMultipleComponent]
    public class DragonFireAnimation : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        public new ParticleSystem particleSystem;

        private void Reset()
        {
            animator = GetComponent<Animator>();
            particleSystem = GetComponentInChildren<ParticleSystem>(includeInactive: true);
        }

        #endregion

        private void Awake()
        {
            particleSystem.Stop();
        }

        public void PlayFireAnimation()
        {
            animator.CrossFade("Attack FireBall", 0.1f);
            particleSystem.gameObject.SetActive(true);
            particleSystem.Stop();
            particleSystem.Play();
        }
    }
}