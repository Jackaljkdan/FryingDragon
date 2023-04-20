using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators
{
    [DisallowMultipleComponent]
    public abstract class AbstractRotationActuator : MonoBehaviour
    {
        #region Inspector

        public float speed = 0.8f;

        [DebugField]
        public Vector2 input;

        [RuntimeField]
        public float initialSpeed;

        #endregion

        protected virtual void Awake()
        {
            initialSpeed = speed;
        }

        public void Rotate(Vector2 input)
        {
            this.input = input;
            RotateProtected(input);
        }

        protected abstract void RotateProtected(Vector2 input);
    }
}