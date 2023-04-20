using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class ParticleSystemTweening
    {
        public static Tween DORadius(this ParticleSystem.ShapeModule self, float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => self.radius,
                val => self.radius = val,
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }

        public static Tween DORadiusThickness(this ParticleSystem.ShapeModule self, float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => self.radiusThickness,
                val => self.radiusThickness = val,
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }
    }
}