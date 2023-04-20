using DG.Tweening;
using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attention
{
    [DisallowMultipleComponent]
    public class Shake : MonoBehaviour
    {
        #region Inspector

        public float intensityMultiplier = 1f;

        public float frequencyMultiplier = 1f;

        public float period = 2.6f;

        public float timeOffset = 0;

        public float seedX = 0f;

        public float seedY = 1f;

        public float seedZ = 2f;

        public Vector3 mask = new Vector3(1, 1, 0);

        public bool restoreInitialPositionOnDisable = false;

        #endregion

        [HideInInspector]
        public Vector3 initialLocalPosition;

        private float time;

        private void Start()
        {
            time = timeOffset;
            initialLocalPosition = transform.localPosition;
        }

        private void OnDisable()
        {
            if (restoreInitialPositionOnDisable)
                transform.localPosition = initialLocalPosition;
        }

        private void Update()
        {
            time += Time.deltaTime * frequencyMultiplier;

            transform.localPosition = initialLocalPosition + new Vector3(
                GetRandom(seedX) * intensityMultiplier * mask.x,
                GetRandom(seedY) * intensityMultiplier * mask.y,
                GetRandom(seedZ) * intensityMultiplier * mask.z
            );
        }

        private float GetRandom(float seed)
        {
            float x = seed;
            float y = time;

            // TODO: possibile ottimizzazione: questa funzione di noise è parecchio costosa
            return noise.snoise(new float2(x, y));
        }

        public Tween DOFrequency(float value, float seconds)
        {
            Tween tween = DOTween.To(
                () => frequencyMultiplier,
                val => frequencyMultiplier = val,
                value,
                seconds
            );

            tween.SetTarget(this);

            return tween;
        }

        public Tween DOIntensity(float value, float seconds)
        {
            Tween tween = DOTween.To(
                () => intensityMultiplier,
                val => intensityMultiplier = val,
                value,
                seconds
            );

            tween.SetTarget(this);

            return tween;
        }
    }
}