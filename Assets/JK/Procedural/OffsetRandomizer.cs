using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class OffsetRandomizer : AbstractRandomizer
    {
        #region Inspector

        public RangeStruct xRange;
        public RangeStruct yRange;
        public RangeStruct zRange;

        [RuntimeField]
        public Vector3 initialLocalPosition;

        #endregion

        private void Awake()
        {
            initialLocalPosition = transform.localPosition;
        }

        public override void Randomize()
        {
            Vector3 randomOffset = new Vector3(
                xRange.RandomlySample(),
                yRange.RandomlySample(),
                zRange.RandomlySample()
            );

            transform.localPosition = initialLocalPosition + randomOffset;
        }
    }
}