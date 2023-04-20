using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public abstract class AbstractIntensitySystem : MonoBehaviour
    {
        #region Inspector

        [Range(0, 1)]
        [RuntimeField]
        public float intensity;

        public float baselineIntensity = 0;

        public float lerp = 0.01f;

        public float offThreshold = 0.01f;

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
                return;

            AdjustToIntensity();
        }

        #endregion

        protected float highestRequestedIntensity;

        protected virtual void OnEnable()
        {
            intensity = baselineIntensity;
            highestRequestedIntensity = baselineIntensity;
        }

        protected virtual void LateUpdate()
        {
            intensity = Mathf.Lerp(intensity, highestRequestedIntensity, TimeUtils.AdjustToFrameRate(lerp));

            if (intensity > offThreshold)
            {
                OnCrossOffThreshold(active: true);
            }
            else if (highestRequestedIntensity == 0)
            {
                intensity = 0;
                OnCrossOffThreshold(active: false);
            }

            AdjustToIntensity();

            highestRequestedIntensity = baselineIntensity;
        }

        protected abstract void OnCrossOffThreshold(bool active);

        protected abstract void AdjustToIntensity();

        public void RequestIntensity(float requestedIntensity)
        {
            if (requestedIntensity > highestRequestedIntensity)
                highestRequestedIntensity = requestedIntensity;
        }
    }
}