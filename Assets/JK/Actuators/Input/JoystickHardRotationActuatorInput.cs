using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class JoystickHardRotationActuatorInput : MonoBehaviour
    {
        #region Inspector

        public string leftRightAxis = "RightJoyX";

        public float threshold = 0.9999f;

        public float deadFlick = 0.2f;

        public float flickSeconds = 0.1f;

        public float speedMultiplier = 10;

        public float rotateSeconds = 0.1f;

        public AbstractRotationActuator actuator;

        [RuntimeField]
        public bool canFlick;

        [RuntimeField]
        public float elapsedFlickSeconds;

        private void Reset()
        {
            actuator = GetComponent<AbstractRotationActuator>();
        }

        #endregion

        private Tween tween;

        private void Start()
        {
            canFlick = false;
        }

        private void Update()
        {
            float axis = UnityEngine.Input.GetAxis(leftRightAxis);

            if (InputUtils.DeadFilter(axis, deadFlick) == 0)
            {
                canFlick = true;
                elapsedFlickSeconds = 0;
            }

            if (canFlick)
            {
                elapsedFlickSeconds += Time.deltaTime;

                float undeadAxis = InputUtils.DeadFilter(axis, threshold);

                if (undeadAxis != 0)
                {
                    DORotate(axis >= 0);
                    canFlick = false;
                }

                if (elapsedFlickSeconds > flickSeconds)
                    canFlick = false;
            }
        }

        private Tween DORotate(bool right)
        {
            tween?.Kill();

            float t = 0;

            tween = DOTween.To(
                () => t,
                val =>
                {
                    t = val;
                    actuator.Rotate(new Vector2(right ? speedMultiplier : -speedMultiplier, 0));
                },
                1,
                rotateSeconds
            );

            tween.SetTarget(this);

            return tween;
        }
    }
}