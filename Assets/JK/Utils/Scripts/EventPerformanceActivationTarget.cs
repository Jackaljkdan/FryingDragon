using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class EventPerformanceActivationTarget : AbstractPerformanceActivationTarget
    {
        #region Inspector

        public UnityEvent onShouldActivate = new UnityEvent();
        public UnityEvent onShouldDeactivate = new UnityEvent();

        #endregion

        protected override void SetActiveForPerformanceProtected(bool active)
        {
            if (active)
                onShouldActivate.Invoke();
            else
                onShouldDeactivate.Invoke();
        }
    }
}