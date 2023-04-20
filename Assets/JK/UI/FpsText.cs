using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class FpsText : MonoBehaviour
    {
        #region Inspector

        public Text text;

        public float measureSeconds = 0.25f;

        [RuntimeField]
        public int framesCount;

        [RuntimeField]
        public float elapsedMeasureSeconds;

        private void Reset()
        {
            text = GetComponent<Text>();
        }

        #endregion

        private void OnEnable()
        {
            framesCount = 0;
            elapsedMeasureSeconds = 0;
        }

        private void Update()
        {
            framesCount++;
            elapsedMeasureSeconds += Time.deltaTime;

            if (elapsedMeasureSeconds >= measureSeconds)
            {
                text.text = $"{framesCount / elapsedMeasureSeconds:0}f {(1000 * elapsedMeasureSeconds / framesCount):0.0}ms";
                OnEnable();
            }
        }
    }
}