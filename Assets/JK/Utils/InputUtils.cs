using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class InputUtils
    {
        public static bool GetAnyShift()
        {
            return UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.RightShift);
        }

        public static bool GetAnyShiftDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.LeftShift) || UnityEngine.Input.GetKeyDown(KeyCode.RightShift);
        }

        public static bool GetAnyControl()
        {
            return UnityEngine.Input.GetKey(KeyCode.LeftControl) || UnityEngine.Input.GetKey(KeyCode.RightControl);
        }

        public static bool GetAnyControlDown()
        {
            return UnityEngine.Input.GetKeyDown(KeyCode.LeftControl) || UnityEngine.Input.GetKeyDown(KeyCode.RightControl);
        }

        public static float DeadFilter(float value, float dead)
        {
            if (value <= -dead)
                return value;

            if (value >= dead)
                return value;

            return 0;
        }
    }
}