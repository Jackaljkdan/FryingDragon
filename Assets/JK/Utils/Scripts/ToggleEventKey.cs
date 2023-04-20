using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public class ToggleEventKey : MonoBehaviour
    {
        #region Inspector

        public bool starsOn = false;

        public KeyCode key = KeyCode.I;

        public UnityEvent onTurnOn = new UnityEvent();
        public UnityEvent onTurnOff = new UnityEvent();

        [RuntimeField]
        public bool isOn;

        #endregion

        private void Start()
        {
            isOn = starsOn;
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(key))
            {
                isOn = !isOn;

                if (isOn)
                    onTurnOn.Invoke();
                else
                    onTurnOff.Invoke();
            }
        }
    }
}