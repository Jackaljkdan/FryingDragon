using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    public abstract class AbstractMovementActuatorInput : MonoBehaviour
    {
        #region Inspector

        public AbstractMovementActuatorBehaviour actuator;

        [Tooltip("Override user input as if he is providing zero")]
        public bool zeroInput = false;

        protected virtual void Reset()
        {
            actuator = GetComponent<AbstractMovementActuatorBehaviour>();
        }

        #endregion

        private void Update()
        {
            actuator.Move(GetInput());
        }

        public abstract Vector3 GetInput();
    }
}