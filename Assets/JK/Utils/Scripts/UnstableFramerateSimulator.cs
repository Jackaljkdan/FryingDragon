using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Utils
{
    [DisallowMultipleComponent]
    public class UnstableFramerateSimulator : MonoBehaviour
    {
        #region Inspector

        public int maxFramerate = 30;
        public int minFramerate = 10;

        #endregion

        private void Update()
        {
            Application.targetFrameRate = UnityEngine.Random.Range(minFramerate, maxFramerate);
        }
    }
}