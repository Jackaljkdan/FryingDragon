using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    public abstract class AbstractCurvedIntensity : MonoBehaviour
    {
        #region Inspector

        public AnimationCurve curve;

        public float loopSeconds = 1;

        [RuntimeHeader]
        public float initialIntensity;

        [RuntimeField]
        public float t;

        #endregion

        protected virtual void Awake()
        {
            initialIntensity = GetIntensity();
        }

        protected void Update()
        {
            t = (t + Time.deltaTime / loopSeconds) % 1;
            float curveEval = curve.Evaluate(t);
            SetIntensity(curveEval * initialIntensity);
        }

        protected abstract float GetIntensity();

        protected abstract void SetIntensity(float value);
    }
}