using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public static class KeyframeUtils
    {
        public static Keyframe Create(float time, float value, float inTangent, float outTanget, float inWeight, float outWeight, WeightedMode weightedMode)
        {
            var k = new Keyframe(time, value, inTangent, outTanget, inWeight, outWeight);
            k.weightedMode = weightedMode;
            return k;
        }

        public static string ToDetailedString(this Keyframe self)
        {
            return $"t: {self.time} v: {self.value} wm: {self.weightedMode} ot: {self.outTangent} ow: {self.outWeight} it: {self.inTangent} iw: {self.inWeight}";
        }

        public static string ToRepresentation(this Keyframe self)
        {
            return $"KeyframeUtils.Create({self.time}f, {self.value}f, {self.inTangent}f, {self.outTangent}f, {self.inWeight}f, {self.outWeight}f, WeightedMode.{self.weightedMode})";
        }
    }
}