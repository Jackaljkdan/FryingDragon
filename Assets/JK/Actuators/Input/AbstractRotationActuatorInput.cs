using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    public abstract class AbstractRotationActuatorInput : MonoBehaviour
    {
        #region Inspector

        public AbstractRotationActuator actuator;

        [Tooltip("Override user input as if he is providing zero")]
        public bool zeroInput = false;

        private void Reset()
        {
            actuator = GetComponent<AbstractRotationActuator>();
        }

        #endregion

        private void Update()
        {
            actuator.Rotate(GetInput());
        }

        public abstract Vector2 GetInput();
    }
}