using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public abstract class AbstractRelaxed : MonoBehaviour
    {
        #region Inspector

        public float updateSeconds = 1;

        public bool stopWhenDisabled = true;

        #endregion

        protected virtual void OnEnable()
        {
            if (!IsInvoking(nameof(RelaxedUpdate)))
                InvokeRepeating(nameof(RelaxedUpdate), UnityEngine.Random.Range(0, updateSeconds), updateSeconds);
        }

        protected virtual void OnDisable()
        {
            if (stopWhenDisabled)
                ForceStop();
        }

        public void ForceStop()
        {
            CancelInvoke(nameof(RelaxedUpdate));
        }

        public abstract void RelaxedUpdate();
    }
}