using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class AxisMovementActuatorInput : AbstractMovementActuatorInput
    {
        #region Inspector

        public AxisClass horizontalAxis = new AxisClass("Horizontal", snap: true);
        public AxisClass verticalAxis = new AxisClass("Vertical", snap: true);

        public AnimationCurve curve;

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsDesktopBuild)
                Destroy(this);
        }

        private void OnEnable()
        {
            horizontalAxis.value = 0;
            verticalAxis.value = 0;
        }

        public override Vector3 GetInput()
        {
            Vector3 input;

            if (!zeroInput)
            {
                input = new Vector3(
                    curve.EvaluateAxis(horizontalAxis.UpdateAndGetValue()),
                    0,
                    curve.EvaluateAxis(verticalAxis.UpdateAndGetValue())
                );
            }
            else
            {
                input = new Vector3(
                    horizontalAxis.UpdateAndGetValue(overridingInput: 0),
                    0,
                    verticalAxis.UpdateAndGetValue(overridingInput: 0)
                );
            }

            input = Vector3.ClampMagnitude(input, 1);

            return input;
        }
    }
}