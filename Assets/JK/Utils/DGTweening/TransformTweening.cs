using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class TransformTweening
    {
        public static Tween DODynamicMove(this Transform self, Func<Vector3> targetPositionGetter, float seconds)
        {
            float t = 0;

            Tween tween = DOTween.To(
                () => t,
                val =>
                {
                    if (t == 1)
                    {
                        self.position = targetPositionGetter();
                    }
                    else
                    {
                        Vector3 target = targetPositionGetter();

                        // lerp(a, b, t) = a + (b - a) * t
                        // t + x * (1 - t) = val
                        // x = (val - t) / (1 - t)
                        float x = (val - t) / (1 - t);
                        self.position = Vector3.Lerp(self.position, target, x);
                    }

                    t = val;
                },
                1,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }

        public static Tween DODynamicRotate(this Transform self, Func<Quaternion> targetRotationGetter, float seconds, RotateMode rotateMode = RotateMode.Fast)
        {
            float t = 0;

            Tween tween = DOTween.To(
                () => t,
                val =>
                {
                    if (t == 1)
                    {
                        self.rotation = targetRotationGetter();
                    }
                    else
                    {
                        Quaternion target = targetRotationGetter();

                        float x = (val - t) / (1 - t);
                        self.rotation = Quaternion.Lerp(self.rotation, target, x);
                    }

                    t = val;
                },
                1,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }
    }
}