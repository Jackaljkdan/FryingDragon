using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public abstract class AbstractPerformanceActivationTarget : MonoBehaviour
    {
        #region Inspector

        public bool startsActive = false;

        [RuntimeField]
        public int activationRequests = 0;

        [ContextMenu("SetActiveForPerfomance")]
        public void SetActiveForPerformanceFromEditor()
        {
            if (Application.isPlaying)
                SetActiveForPerformance(true);
        }

        [ContextMenu("SetInactiveForPerfomance")]
        public void SetInactiveForPerformanceFromEditor()
        {
            if (Application.isPlaying)
                SetActiveForPerformance(false);
        }

        #endregion

        protected virtual void Start()
        {
            SetActiveForPerformance(startsActive);
        }

        protected abstract void SetActiveForPerformanceProtected(bool active);

        public void SetActiveForPerformance(bool active)
        {
            int prevActivationRequests = activationRequests;

            if (active)
                activationRequests++;
            else if (activationRequests > 0)
                activationRequests--;

            if (prevActivationRequests == 0 && activationRequests == 1)
                SetActiveForPerformanceProtected(true);
            else if (prevActivationRequests == 1 && activationRequests == 0)
                SetActiveForPerformanceProtected(false);

        }
    }
}