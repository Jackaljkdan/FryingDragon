using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class CharacterControllerMovementActuator : AbstractMovementActuatorBehaviour
    {
        #region Inspector

        public CharacterController characterController;

        private void Reset()
        {
            characterController = GetComponentInChildren<CharacterController>();
        }

        #endregion

        protected override void MoveProtected(Vector3 direction)
        {
            Vector3 velocity = direction * Speed;
            characterController.SimpleMove(DirectionReference.TransformDirection(velocity));
        }
    }
}