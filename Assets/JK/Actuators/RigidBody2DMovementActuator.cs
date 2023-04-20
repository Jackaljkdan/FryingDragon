using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public class RigidBody2DMovementActuator : AbstractMovementActuatorBehaviour
    {
        #region Inspector

        public new Rigidbody2D rigidbody;

        private void Reset()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        #endregion

        protected override void MoveProtected(Vector3 direction)
        {
            // TODO: dovrei accumulare l'input ed eseguirlo in fixed update?
            Vector2 direction2D = new Vector2(direction.x, direction.z);
            rigidbody.velocity = DirectionReference.TransformDirection(direction2D) * Speed;
        }
    }
}