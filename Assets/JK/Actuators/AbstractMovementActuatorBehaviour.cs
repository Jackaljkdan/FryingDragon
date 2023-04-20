using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public abstract class AbstractMovementActuatorBehaviour : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float _speed = 3;

        public Transform _directionReference;

        [SerializeField, DebugField]
        protected Vector3 input;

        private void Reset()
        {
            DirectionReference = transform;
        }

        #endregion

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        public Transform DirectionReference
        {
            get => _directionReference;
            set => _directionReference = value;
        }

        public void Move(Vector3 direction)
        {
            input = direction;
            MoveProtected(direction);
        }

        protected abstract void MoveProtected(Vector3 direction);
    }
}