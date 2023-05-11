using JK.UI;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Actuators.Input
{
    [DisallowMultipleComponent]
    public class AbstractMovementInput : MonoBehaviour
    {
        #region Inspector

        public AbstractMovement movement;

        [Tooltip("Override user input as if he is providing zero")]
        public bool zeroInput = false;

        public UnityEvent onEnable = new UnityEvent();
        public UnityEvent onDisable = new UnityEvent();

        protected virtual void Reset()
        {
            movement = GetComponent<AbstractMovement>();
        }

        #endregion

        protected virtual void OnEnable()
        {
            onEnable.Invoke();
        }

        protected virtual void OnDisable()
        {
            onDisable.Invoke();
        }
    }
}