using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attention
{
    public abstract class AbstractFlicker : MonoBehaviour
    {
        #region Inspector

        public float flickersPerSecond = 1;
        public float minStaySeconds = 0.1f;
        public float timeVariancePercentage = 0.5f;
        public float rescheduleThresholdPercentage = 0.2f;

        [Tooltip("Multiplier to get min flicker value")]
        public float min;
        [Tooltip("Multiplier to get max flicker value")]
        public float max;

        [Header("Runtime")]

        public float nextFlickerTime = 0;

        #endregion

        private float initialValue;

        protected abstract float GetFlickeredValue();

        protected abstract void SetFlickeredValue(float value);

        protected virtual void Awake()
        {
            // prima che qualcuno la cambi
            initialValue = GetFlickeredValue();
        }

        protected virtual void Start()
        {
            ForceRescheduleNextFlicker();
        }

        private void Update()
        {
            if (flickersPerSecond <= 0)
                return;

            if (Time.time < nextFlickerTime)
                return;

            SetFlickeredValue(initialValue * min);
            ForceRescheduleNextFlicker();
            SetToMaxAfterRandomWait();
        }

        private void SetToMaxAfterRandomWait()
        {
            float delta = minStaySeconds * timeVariancePercentage;
            float waitSeconds = UnityEngine.Random.Range(minStaySeconds - delta, minStaySeconds + delta);
            this.RunAfterSeconds(waitSeconds, () => SetFlickeredValue(initialValue * max));
        }

        public void ForceRescheduleNextFlicker()
        {
            float avgWaitSeconds = 1 / flickersPerSecond;
            float delta = avgWaitSeconds * timeVariancePercentage;
            nextFlickerTime = Time.time + UnityEngine.Random.Range(avgWaitSeconds - delta, avgWaitSeconds + delta);
        }

        public void RescheduleNextFlickerIfNeeded()
        {
            float waitSeconds = nextFlickerTime - Time.time;

            if (waitSeconds < 1)
                return;

            float avgWaitSeconds = 1 / flickersPerSecond;
            float delta = Math.Abs(waitSeconds - avgWaitSeconds);

            if (delta > avgWaitSeconds * rescheduleThresholdPercentage)
                ForceRescheduleNextFlicker();
        }
    }
}