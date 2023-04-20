using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Attention
{
    [DisallowMultipleComponent]
    public class LateNoisyRotation : MonoBehaviour
    {
        #region Inspector

        public float intensityMultiplier = 10f;

        public float frequencyMultiplier = 1f;

        public float timeOffset = 0;

        public float seedX = 0f;

        public float seedY = 1f;

        public float seedZ = 2f;

        public Vector3 mask = new Vector3(1, 1, 1);

        #endregion

        private float time;

        private void Start()
        {
            time = timeOffset;
        }

        private void LateUpdate()
        {
            time += Time.deltaTime * frequencyMultiplier;

            transform.localRotation *= Quaternion.Euler(
                GetRandom(seedX) * intensityMultiplier * mask.x,
                GetRandom(seedY) * intensityMultiplier * mask.y,
                GetRandom(seedZ) * intensityMultiplier * mask.z
            );
        }

        private float GetRandom(float seed)
        {
            float x = seed;
            float y = time;

            return noise.snoise(new float2(x, y));
        }
    }
}