using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class TransformRotationActuator : AbstractRotationActuator
    {
        #region Inspector

        public Transform horizontalTarget;
        public Transform verticalTarget;

        public float lowerBound = 86;
        public float upperBound = 89;

        [RuntimeField]
        public float x;

        private void Reset()
        {
            horizontalTarget = transform;
            verticalTarget = transform;
        }

        #endregion

        private void OnEnable()
        {
            // ensure bounds are respected
            RotateX(0);
        }

        protected override void RotateProtected(Vector2 input)
        {
            Vector3 adjusted = TimeUtils.AdjustToFrameRate(speed * input);

            horizontalTarget.RotateAround(horizontalTarget.position, Vector3.up, adjusted.x);
            RotateX(-adjusted.y);
        }

        private void RotateX(float amount)
        {
            x = verticalTarget.localEulerAngles.x;

            if (x > 180)
                x -= 360;

            x = Mathf.Clamp(x + amount, -upperBound, lowerBound);
            verticalTarget.localEulerAngles = verticalTarget.localEulerAngles.WithX(x);
        }
    }
}