using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class DisableAnimatorOnIdle : MonoBehaviour
    {
        #region Inspector

        public Animator animator;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        #endregion

        public void OnIdle()
        {
            animator.enabled = false;
        }
    }
}