using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class AnimationCurveUtils
    {
        public static string ToRepresentation(this AnimationCurve self)
        {
            List<string> keyframes = new List<string>(self.length);

            foreach (var k in self.keys)
                keyframes.Add(k.ToRepresentation());

            return "new AnimationCurve(\n    " + string.Join(",\n    ", keyframes) + "\n);";
        }

        public static float EvaluateAxis(this AnimationCurve self, float axis)
        {
            return self.Evaluate(Mathf.Abs(axis)) * Mathf.Sign(axis);
        }
    }
}