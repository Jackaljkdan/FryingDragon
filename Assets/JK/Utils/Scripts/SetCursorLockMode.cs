using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class SetCursorLockMode : MonoBehaviour
    {
        #region Inspector

        public bool onlyInBuild = true;

        public CursorLockMode defaultMode = CursorLockMode.Locked;

        #endregion

        private void Start()
        {
            SetMode(defaultMode);
        }

        public void SetMode(CursorLockMode mode)
        {
            if (!onlyInBuild || !PlatformUtils.IsEditor)
                Cursor.lockState = mode;
        }

        public void SetCursorModeToNone()
        {
            SetMode(CursorLockMode.None);
        }

        public void SetCursorModeToLocked()
        {
            SetMode(CursorLockMode.Locked);
        }

        public void SetCursorModeToConfined()
        {
            SetMode(CursorLockMode.Confined);
        }

        public void SetCursorModeToDefault()
        {
            SetMode(defaultMode);
        }
    }
}