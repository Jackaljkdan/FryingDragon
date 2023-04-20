using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public class AxisAsButtonClass
    {
        #region Inspector

        public string axis;

        [RuntimeField]
        public bool isDown;

        #endregion

        public AxisAsButtonClass()
        {

        }

        public AxisAsButtonClass(string axis)
        {
            this.axis = axis;
        }

        public bool GetAxis(out float value)
        {
            value = UnityEngine.Input.GetAxis(axis);

            if (value != 0 && !isDown)
                isDown = true;
            else if (value == 0)
                isDown = false;

            return isDown;
        }

        public bool GetAxis()
        {
            return GetAxis(out _);
        }

        public bool GetAxisDown()
        {
            bool wasDown = isDown;
            GetAxis();
            return !wasDown && isDown;
        }

        public bool GetAxisUp()
        {
            bool wasDown = isDown;
            GetAxis();
            return wasDown && !isDown;
        }
    }
}