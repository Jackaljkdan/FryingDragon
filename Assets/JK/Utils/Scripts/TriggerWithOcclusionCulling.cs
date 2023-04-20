using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class TriggerWithOcclusionCulling : OcclusionCullingListener
    {
        #region Inspector

        public UnityEvent onTriggerEnterOrVisible = new UnityEvent();

        public UnityEvent onTriggerExitAndInvisible = new UnityEvent();

        [RuntimeField]
        public bool insideTrigger = false;

        #endregion

        protected override void Start()
        {
            onVisible.AddListener(OnVisible);
            onInvisible.AddListener(OnInvisible);

            base.Start();
        }

        private void OnTriggerEnter(Collider other)
        {
            insideTrigger = true;
            onTriggerEnterOrVisible.Invoke();
        }

        private void OnVisible()
        {
            onTriggerEnterOrVisible.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            insideTrigger = false;

            if (!IsVisible())
                onTriggerExitAndInvisible.Invoke();
        }

        private void OnInvisible()
        {
            if (!insideTrigger)
                onTriggerExitAndInvisible.Invoke();
        }
    }
}