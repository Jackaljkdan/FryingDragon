using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    [LateExecutionOrder]
    public class CopyCameraBobOffsetScript : MonoBehaviour
    {
        #region Inspector

        public float multiplier = 1;

        public CameraBobScript cameraBob;

        public Transform target;

        [RuntimeField]
        public float initialY;

        private void Reset()
        {
            cameraBob = this.GetComponentInParentOrChildren<CameraBobScript>();
            target = transform;
        }

        #endregion

        private void Awake()
        {
            initialY = target.localPosition.y;
        }

        private void Update()
        {
            target.localPosition = target.localPosition.WithY(initialY + cameraBob.offset * multiplier);
        }
    }
}