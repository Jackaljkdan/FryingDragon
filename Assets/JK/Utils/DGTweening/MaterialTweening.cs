using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils.DGTweening
{
    public static class MaterialTweening
    {
        public static Tween DOEmissionColor(this Material self, Color color, float seconds)
        {
            int emissionColorId = Shader.PropertyToID("_EmissionColor");

            Tween tween = DOTween.To(
                () => self.GetColor(emissionColorId),
                val => self.SetColor(emissionColorId, val),
                color,
                seconds
            );

            tween.SetTarget(self);

            return tween;
        }
    }
}