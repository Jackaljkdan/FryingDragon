using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class EnvironmentLighting : MonoBehaviour
    {
        #region Inspector

        [RuntimeHeader]

        public Color initialAmbientLight;

        #endregion

        private void Awake()
        {
            // prima che qualcuno la cambi
            initialAmbientLight = RenderSettings.ambientLight;
        }

        public Tween DOAmbientLight(Color color, float seconds)
        {
            var tween = DOTween.To(
                () => RenderSettings.ambientLight,
                val => RenderSettings.ambientLight = val,
                color,
                seconds
            );

            tween.SetTarget(this);

            return tween;
        }
    }
}