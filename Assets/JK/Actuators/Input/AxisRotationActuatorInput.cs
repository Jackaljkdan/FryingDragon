using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class AxisRotationActuatorInput : AbstractRotationActuatorInput
    {
        #region Inspector fields

        public AxisClass leftRightAxis = new AxisClass("Mouse X", 1, snap: false);
        public AxisClass upDownAxis = new AxisClass("Mouse Y", 1, snap: false);

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsDesktopBuild)
                Destroy(this);
        }

        private void OnEnable()
        {
            leftRightAxis.value = 0;
            upDownAxis.value = 0;
        }

        public override Vector2 GetInput()
        {
            if (!zeroInput)
            {
                return new Vector2(
                    leftRightAxis.UpdateAndGetValue(),
                    upDownAxis.UpdateAndGetValue()
                );
            }
            else
            {
                return new Vector2(
                    leftRightAxis.UpdateAndGetValue(overridingInput: 0),
                    upDownAxis.UpdateAndGetValue(overridingInput: 0)
                );
            }
        }
    }
}