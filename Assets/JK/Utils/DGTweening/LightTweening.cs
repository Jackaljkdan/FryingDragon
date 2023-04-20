using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class LightTweening
    {
        public static Tween DORange(this Light self, float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => self.range,
                val => self.range = val,
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }

        public static Tween DOShadowsNearPlane(this Light self, float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => self.shadowNearPlane,
                val => self.shadowNearPlane = val,
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }
    }
}