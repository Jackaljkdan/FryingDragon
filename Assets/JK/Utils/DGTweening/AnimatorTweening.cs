using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class AnimatorTweening
    {
        public static Tween DOFloat(this Animator self, string name, float endValue, float seconds)
        {
            int id = Animator.StringToHash(name);
            return self.DOFloat(id, endValue, seconds);
        }

        public static Tween DOFloat(this Animator self, int id, float endValue, float seconds)
        {
            Tween tween = DOTween.To(
                () => self.GetFloat(id),
                val => self.SetFloat(id, val),
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }

        public static Tween DOLayerWeight(this Animator self, int layerIndex, float endValue, float seconds)
        {
            Tween tween = DOTween.To(
                () => self.GetLayerWeight(layerIndex),
                val => self.SetLayerWeight(layerIndex, val),
                endValue,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }

        public static Tween DOSpeed(this Animator self, float endValue, float seconds)
        {
            Tween tween = DOTween.To(
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