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
            animator.CrossFade("PackEnd", 0.1f);
        }

        public void PlayHorrorLoop()
        {
            CancelInvoke(nameof(StopPack));
            animator.CrossFade("HorrorLoop", 0.1f);
        }

        public void PlayHorrorQuick()
        {
            CancelInvoke(nameof(StopPack));
            animator.CrossFade("HorrorQuick", 0.1f);
        }

        public void PlayIdle()
        {
            CancelInvoke(nameof(StopPack));
            animator.CrossFade("Idle", 0.1f);
        }
    }
}