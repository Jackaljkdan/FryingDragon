using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    public abstract class AbstractMovementSpeedMultiplier : MonoBehaviour
    {
        #region Inspector

        public AbstractMovementActuatorBehaviour actuator;

        public float multiplier = 2;

        public float lerp = 0.1f;

        public float joyRunThreshold = 0.95f;

        [RuntimeField]
        public float initialSpeed;

        private void Reset()
        {
            actuator = GetComponent<AbstractMovementActuatorBehaviour>();
        }

        #endregion

        protected virtual void Awake()
        {
            initialSpeed = actuator.Speed;
        }

        public void UpdateSpeed(float targetMultiplier)
        {
            actuator.Speed = Mathf.Lerp(actuator.Speed, initialSpeed * targetMultiplier, TimeUtils.AdjustToFrameRate(lerp));
        }
    }
}