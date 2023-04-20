using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [Serializable]
    public struct RangeStruct
    {
        public float min;
        public float max;

        public RangeStruct(float value)
        {
            min = max = value;
        }

        public RangeStruct(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public override string ToString()
        {
            return $"{{{min}, {max}}}";
        }

        public float RandomlySample()
        {
            return UnityEngine.Random.Range(min, max);
        }

        public float Lerp(float t)
        {
            return Mathf.Lerp(min, max, t);
        }

        public static RangeStruct Lerp(RangeStruct a, RangeStruct b, float t)
        {
            return new RangeStruct(
                Mathf.Lerp(a.min, b.min, t),
                Mathf.Lerp(a.max, b.max, t)
            );
        }

        public bool Contains(float distance)
        {
            return distance >= min && distance <= max;
        }
    }
}