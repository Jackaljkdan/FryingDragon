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

        public void PlayPack()
        {
            animator.CrossFade("PackStart", 0.1f);
        }

        public void StopPack()
        {
            animator.CrossFade("PackEnd", 0.1f);
        }

        public void PlayHorrorLoop()
        {
            animator.CrossFade("HorrorLoop", 0.1f);
        }

        public void PlayHorrorQuick()
        {
            animator.CrossFade("HorrorQuick", 0.1f);
        }

        public void PlaySleepLoop()
        {
            animator.CrossFade("SleepLoop", 0.1f);
        }

        public void PlayIdle()
        {
            animator.CrossFade("Idle", 0.1f);
        }
    }
}