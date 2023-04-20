using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attention
{
    [DisallowMultipleComponent]
    public class FlickerLightRange : AbstractFlicker
    {
        #region Inspector

        public new Light light;

        private void Reset()
        {
            light = GetComponentInChildren<Light>();
        }

        #endregion

        protected override float GetFlickeredValue()
        {
            return light.range;
        }

        protected override void SetFlickeredValue(float value)
        {
            light.range = value;
        }
    }
}