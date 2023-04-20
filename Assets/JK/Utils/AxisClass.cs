using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public class AxisClass
    {
        public string axis;

        [Tooltip("Speed to move towards target value")]
        public float deltaPerSecond = 3;
        [Tooltip("In case of opposite input do we jump to zero?")]
        public bool snap = false;

        public float value;

        public AxisClass() { }

        public AxisClass(string axis)
        {
            this.axis = axis;
        }

        public AxisClass(string axis, bool snap)
        {
            this.axis = axis;
            this.snap = snap;
        }

        public AxisClass(string axis, float deltaPerSecond)
        {
            this.axis = axis;
            this.deltaPerSecond = deltaPerSecond;
        }

        public AxisClass(string axis, float deltaPerSecond, bool snap)
        {
            this.axis = axis;
            this.deltaPerSecond = deltaPerSecond;
            this.snap = snap;
        }

        public static float MoveTowardsInput(float value, float input, float deltaPerSecond, bool snap)
        {
            if (snap)
            {
                if (value > 0 && input < 0)
                    value = 0;
                else if (value < 0 && input > 0)
                    value = 0;
            }

            if (input > value)
                value = Mathf.Min(value + deltaPerSecond * Time.deltaTime, input);
            else
                value = Mathf.Max(value - deltaPerSecond * Time.deltaTime, input);

            return value;
        }

        public void Update(float overridingInput)
        {
            value = MoveTowardsInput(value, overridingInput, deltaPerSecond, snap);
        }

        public void Update()
        {
            float input = UnityEngine.Input.GetAxis(axis);
            Update(input);
        }

        public float UpdateAndGetValue()
        {
            Update();
            return value;
        }

        public float UpdateAndGetValue(float overridingInput)
        {
            Update(overridingInput);
            return value;
        }
    }
}