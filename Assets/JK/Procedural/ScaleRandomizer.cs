using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    [DisallowMultipleComponent]
    public class ScaleRandomizer : AbstractRandomizer
    {
        #region Inspector

        public bool uniformlyScale = true;

        [Conditional(nameof(uniformlyScale))]
        public RangeStruct uniformRange = new RangeStruct(0.8f, 1.2f);

        [Conditional(nameof(uniformlyScale), inverse: true)]
        public RangeStruct xRange = new RangeStruct(0.8f, 1.2f);
        [Conditional(nameof(uniformlyScale), inverse: true)]
        public RangeStruct yRange = new RangeStruct(0.8f, 1.2f);
        [Conditional(nameof(uniformlyScale), inverse: true)]
        public RangeStruct zRange = new RangeStruct(0.8f, 1.2f);

        private void Reset()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if (uniformlyScale)
            {
                transform.localScale = Vector3Utils.Create(uniformRange.max);
            }
            else
            {
                transform.localScale = new Vector3(
                    xRange.max,
                    yRange.max,
                    zRange.max
                );
            }

            UndoUtils.SetDirty(transform);
        }

        #endregion

        public override void Randomize()
        {
            if (uniformlyScale)
            {
                float scale = uniformRange.RandomlySample();
                transform.localScale = Vector3Utils.Create(scale);
            }
            else
            {
                transform.localScale = new Vector3(
                    xRange.RandomlySample(),
                    yRange.RandomlySample(),
                    zRange.RandomlySample()
                );
            }
        }
    }
}