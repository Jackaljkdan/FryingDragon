using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class LerpedAxisRotationActuatorInput : AbstractRotationActuatorInput
    {
        #region Inspector fields

        public string leftRightAxis = "Mouse X";
        public string upDownAxis = "Mouse Y";

        public string joystickLeftRightAxis = "RightJoyX";
        public string joystickUpDownAxis = "RightJoyY";

        public AnimationCurve joystickCurve;

        public float joystickSpeedMultiplier = 7.5f;
        public float joystickDead = 0.11f;

        public float lerp = 0.3f;

        [RuntimeField]
        public Vector2 inertia;

        #endregion

        private void Awake()
        {
            if (!PlatformUtils.IsDesktopBuild)
                Destroy(this);
        }

        private void OnEnable()
        {
            inertia = Vector2.zero;
        }

        public override Vector2 GetInput()
        {
            Vector2 input;

            if (!zeroInput)
            {
                input = new Vector2(
                    UnityEngine.Input.GetAxis(leftRightAxis),
                    UnityEngine.Input.GetAxis(upDownAxis)
                );

                if (input.x == 0 && input.y == 0)
                {
                    float undeadRange = 1 - joystickDead;

                    float undeadInputX = InputUtils.DeadFilter(UnityEngine.Input.GetAxis(joystickLeftRightAxis), joystickDead) / undeadRange;
                    float undeadInputY = InputUtils.DeadFilter(UnityEngine.Input.GetAxis(joystickUpDownAxis), joystickDead) / undeadRange;

                    input = new Vector2(
                        joystickSpeedMultiplier * joystickCurve.EvaluateAxis(undeadInputX),
                        joystickSpeedMultiplier * joystickCurve.EvaluateAxis(undeadInputY)
                    );
                }
            }
            else
            {
                input = Vector2.zero;
            }

            float t = PlatformUtils.IsEditor
                ? TimeUtils.AdjustToClampedFrameRate(lerp, clamp: 60)
                : TimeUtils.AdjustToFrameRate(lerp)
            ;

            inertia = Vector2.Lerp(inertia, input, t);

            return inertia;
        }
    }
}