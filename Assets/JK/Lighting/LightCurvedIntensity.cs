using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Lighting
{
    [DisallowMultipleComponent]
    public class LightCurvedIntensity : AbstractCurvedIntensity
    {
        #region Inspector

        public Light target;

        private void Reset()
        {
            target = GetComponent<Light>();
        }

        #endregion

        protected override float GetIntensity()
        {
            return target.intensity;
        }

        protected override void SetIntensity(float value)
        {
            target.intensity = value;
        }
    }
}