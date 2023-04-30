using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class CameraMovement : MonoBehaviour
    {
        #region Inspector

        public Transform target;
        public float smoothSpeed = 0.125f;
        public float minZPosition = -10.3f;

        [DebugField]
        public Vector3 offset;


        #endregion

        private void Awake()
        {
            if (target != null)
            {
                offset = transform.position - target.position;
            }
        }
        private void FixedUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            desiredPosition.z = Mathf.Max(desiredPosition.z, minZPosition);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}