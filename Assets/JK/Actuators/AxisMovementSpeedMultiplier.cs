using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class AxisMovementSpeedMultiplier : AbstractMovementSpeedMultiplier
    {
        #region Inspector

        public string axis = "Run";

        [DebugField]
        public float axisValue;

        [DebugField]
        public float joyX;

        [DebugField]
        public float joyY;

        [DebugField]
        public float joyMagnitude;

        #endregion

        private void Update()
        {
            axisValue = UnityEngine.Input.GetAxisRaw(axis);

            if (UnityEngine.Input.GetAxisRaw(axis) > 0)
            {
                UpdateSpeed(multiplier);
            }
            else
            {
                Vector2 joyVector = new Vector2(
                    UnityEngine.Input.GetAxis("JoyX"),
                    UnityEngine.Input.GetAxis("JoyY")
                );

                joyMagnitude = joyVector.magnitude;

                if (joyMagnitude >= joyRunThreshold)
                    UpdateSpeed(multiplier);
                else
                    UpdateSpeed(1);
            }
        }
    }
}