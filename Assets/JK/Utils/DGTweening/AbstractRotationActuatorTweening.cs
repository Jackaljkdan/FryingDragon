using DG.Tweening;
using JK.Actuators;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class AbstractRotationActuatorTweening
    {
        public static Tween DOSpeed(this AbstractRotationActuator self, float endValue, float seconds)
        {
            var tween = DOTween.To(
                () => self.speed,
                val => self.speed = val,
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }
    }
}