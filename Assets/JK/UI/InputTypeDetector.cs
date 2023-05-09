using JK.Observables;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class InputTypeDetector : MonoBehaviour
    {
        #region Inspector

        public bool switchOnMouseMovement = true;

        public float controllerDead = 0.1f;

        public ObservableProperty<InputType> current = new();

        #endregion

        private void Update()
        {
            if (switchOnMouseMovement)
            {
                if (UnityEngine.Input.GetAxis("Mouse X") != 0 || UnityEngine.Input.GetAxis("Mouse Y") != 0)
                {
                    current.Value = InputType.Keyboard;
                    return;
                }
            }

            if (InputUtils.DeadFilter(UnityEngine.Input.GetAxis("RightHorizontal"), controllerDead) != 0
                || InputUtils.DeadFilter(UnityEngine.Input.GetAxis("RightVertical"), controllerDead) != 0)
            {
                current.Value = InputType.Controller;
                return;
            }
        }
    }
}