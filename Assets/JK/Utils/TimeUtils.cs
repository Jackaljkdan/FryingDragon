using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class TimeUtils
    {
        public static float AdjustToFrameRate(float value, float targetFrameRate = 60)
        {
            return value * targetFrameRate * Time.deltaTime;
        }

        public static float AdjustToClampedFrameRate(float value, float targetFrameRate = 60, float clamp = 40)
        {
            return value * targetFrameRate * Mathf.Clamp(Time.deltaTime, 0, 1 / clamp);
        }

        public static Vector3 AdjustToFrameRate(Vector3 value, float targetFrameRate = 60)
        {
            return targetFrameRate * Time.deltaTime * value;
        }
    }
}