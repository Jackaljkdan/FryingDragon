using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public AudioSource audioSource;

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

        private UnityAction onBreathFireEnd;

        public void PlayFireAnimation(UnityAction onBreathFireEnd = null)
        {
            this.onBreathFireEnd = onBreathFireEnd;

            animator.CrossFade("Breath Fire Start", 0.1f);
            particleSystem.gameObject.SetActive(true);
            particleSystem.Stop();
            particleSystem.Play();
            audioSource.Play();
        }

        public void OnBreathFireEnd()
        {
            onBreathFireEnd?.Invoke();
            onBreathFireEnd = null;
        }

        public bool IsPlayingFireAnimation()
        {
            var hash = Animator.StringToHash("Breath Fire Start");

            return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hash
                || animator.GetNextAnimatorStateInfo(0).shortNameHash == hash
            ;
        }
    }
}