using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public abstract class AbstractMovement : MonoBehaviour
    {
        #region Inspector



        #endregion

        public void Move(Vector3 movementInput)
        {
            Move(movementInput, transform.forward);
        }

        public abstract void Move(Vector3 movementInput, Vector3 forwardInput);
    }
}