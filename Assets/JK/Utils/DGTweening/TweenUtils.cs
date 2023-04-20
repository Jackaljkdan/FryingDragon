using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class TweenUtils
    {
        public static bool IsActiveAndPlaying(this Tween self)
        {
            return self.IsActive() && self.IsPlaying();
        }

        private static Tween dummyPausedTween;

        public static Tween GetDummyPausedTween()
        {
            if (dummyPausedTween == null)
            {
                float x = 42;
                dummyPausedTween = DOTween.To(
                    () => x,
                    val => x = val,
                    3.14f,
                    42
                );

                dummyPausedTween.Pause();
            }

            return dummyPausedTween;
        }

        public static Sequence DOSimultaneously(Tween a, Tween b)
        {
            var seq = DOTween.Sequence();

            seq.Insert(0, a);
            seq.Insert(0, b);

            return seq;
        }
    }
}