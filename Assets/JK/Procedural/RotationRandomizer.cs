using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class RotationRandomizer : AbstractRandomizer
    {
        #region Inspector

        public bool randomizeX = false;
        [Conditional(nameof(randomizeX))]
        public RangeStruct xRange = new RangeStruct(0, 360);

        public bool randomizeY = true;
        [Conditional(nameof(randomizeY))]
        public RangeStruct yRange = new RangeStruct(0, 360);

        public bool randomizeZ = false;
        [Conditional(nameof(randomizeZ))]
        public RangeStruct zRange = new RangeStruct(0, 360);

        #endregion

        public override void Randomize()
        {
            Transform myTransform = transform;

            transform.localEulerAngles = new Vector3(
                randomizeX ? xRange.RandomlySample() : myTransform.localEulerAngles.x,
                randomizeY ? yRange.RandomlySample() : myTransform.localEulerAngles.y,
                randomizeZ ? zRange.RandomlySample() : myTransform.localEulerAngles.z
            );
        }
    }
}