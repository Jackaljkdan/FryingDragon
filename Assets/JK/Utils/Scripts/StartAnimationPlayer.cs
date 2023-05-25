using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class StartAnimationPlayer : MonoBehaviour
    {
        #region Inspector

        public Animator animator;
        public string selectedAnimation;

        #endregion

        private void Start()
        {
            PlaySelectedAnimation();
        }

        public void PlaySelectedAnimation()
        {
            if (animator != null && !string.IsNullOrEmpty(selectedAnimation))
            {
                animator.Play(selectedAnimation);
            }
        }

    }
}