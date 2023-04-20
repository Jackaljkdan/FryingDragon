using JK.Lighting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attention
{
    [DisallowMultipleComponent]
    public class FlickerLightLODIntensity : AbstractFlicker
    {
        #region Inspector

        public LightLOD lightLOD;

        private void Reset()
        {
            lightLOD = GetComponentInChildren<LightLOD>();
        }

        #endregion

        protected override float GetFlickeredValue()
        {
            return lightLOD.intensity;
        }

        protected override void SetFlickeredValue(float value)
        {
            lightLOD.intensity = value;
        }
    }
}