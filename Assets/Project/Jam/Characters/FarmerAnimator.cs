using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam.Characters
{
    [DisallowMultipleComponent]
    public class FarmerAnimator : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        public void PlayPack(float seconds)
        {
            animator.CrossFade("PackStart", 0.1f);
            Invoke(nameof(StopPack), seconds);
        }

        public void StopPack()
        {
            animator.CrossFade("PackStop", 0.1f);
        }

        public void PlayHorrorLoop()
        {
            animator.CrossFade("HorrorLoop", 0.1f);
        }

        public void PlayHorrorQuick()
        {
            animator.CrossFade("HorrorQuick", 0.1f);
        }

        public void PlayIdle()
        {
            animator.CrossFade("Idle", 0.1f);
        }
    }
}