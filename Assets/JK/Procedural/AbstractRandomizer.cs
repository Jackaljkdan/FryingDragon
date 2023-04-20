using JK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Procedural
{
    public abstract class AbstractRandomizer : MonoBehaviour
    {
        #region Inspector

        public bool randomizeOnEnable = true;

        [ContextMenu("Randomize")]
        private void RandomizeInEditMode()
        {
            if (Application.isPlaying)
                Randomize();
        }

        #endregion

        protected virtual void OnEnable()
        {
            if (randomizeOnEnable)
                Randomize();
        }

        public abstract void Randomize();
    }
}